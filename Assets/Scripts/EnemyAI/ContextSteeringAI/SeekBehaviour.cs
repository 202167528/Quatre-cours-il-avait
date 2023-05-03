using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField] private float targetReachedThreshold = 0.5f;

    [SerializeField] private bool showGizmo = true;
    
    private bool reachedLastTarget = true;
    
    // paramètres gizmo
    public Vector3 targetPositionCached;
    private float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        // Si on n'a pas de target, on arrête de seek
        // Sinon on set un nouveau target
        if (reachedLastTarget)
        {
            if (aiData.targets == null || aiData.targets.Count <= 0)
            {
                aiData.currentTarget = null;
                return (danger, interest);
            }
            else
            {
                reachedLastTarget = false;
                aiData.currentTarget = aiData.targets.OrderBy
                    (target => Vector3.Distance(target.position, transform.position)).FirstOrDefault();
            }
        }

        // Garde en mémoire la dernière position seulement si on voit toujours le target (si le targets collection n'est pas empty)
        if (aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
            targetPositionCached = aiData.currentTarget.position;

        // Trouve si on a atteint le target
        if (Vector3.Distance(transform.position, targetPositionCached) < targetReachedThreshold)
        {
            reachedLastTarget = true;
            aiData.currentTarget = null;
            return (danger, interest);
        }

        // Si on ne l'a pas encore atteint, on fait la logique pour trouver les directions
        var directionToTarget = (targetPositionCached - transform.position).normalized;
        for (var i = 0; i < interest.Length; i++)
        {
            var result = Vector3.Dot(directionToTarget, Directions.eightDirections[i]);

            // Accepte seulement les directions à moins de 90 degré de la direction 
            if (result > 0)
            {
                var valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }

            }
        }
        interestsTemp = interest;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {

        if (showGizmo == false)
            return;
        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestsTemp != null)
        {
            if (interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i] * 2);
                }
                if (reachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPositionCached, 0.1f);
                }
            }
        }
    }
}
