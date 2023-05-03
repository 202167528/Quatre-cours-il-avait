using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour
{
    [SerializeField] private AIData aiData;

    [SerializeField] private bool showGizmos = true;

    public Vector3 PatrolPointsPosition { get; set; }

    private float[] interestsTemp;
    private Vector3 resultDirection = Vector3.zero;
    private float rayLength = 2;

    public Vector3 GetDirectionToMove(List<SteeringBehaviour> behaviours, AIData aiData)
    {
        var danger = new float[Directions.eightDirections.Capacity]; 
        var interest = new float[Directions.eightDirections.Capacity];

        // Loop chaque behaviour
        foreach (var behaviour in behaviours)
        {
            (danger, interest) = behaviour.GetSteering(danger, interest, aiData);
        }

        // Si le target est null, trouve les intérêts vers le patrol point suivant
        if (aiData.currentTarget == null)
        {
            var directionToPatrolPoints = (PatrolPointsPosition - transform.position).normalized;

            for (var i = 0; i < interest.Length; i++)
            {
                interest[i] = Vector3.Dot(directionToPatrolPoints, Directions.eightDirections[i]);
            }
            interestsTemp = interest;
        }

        // Soustrait la valeur de danger sur le tableau de interest
        for (var i = 0; i < Directions.eightDirections.Capacity; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }
        
        // Renvoie la valeur moyenne de la direction
        var outputDirection = Vector3.zero;
        for (var i = 0; i < Directions.eightDirections.Capacity; i++)
        {
            outputDirection += Directions.eightDirections[i] * interest[i];
        }
        outputDirection.Normalize();

        resultDirection = outputDirection;
 
        // Retourne la direction de mouvement choisi
        return resultDirection;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && showGizmos)
        {
            if (interestsTemp != null && aiData.currentTarget == null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i] * 2);
                }
            }
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDirection * rayLength);
        }
    }
}
