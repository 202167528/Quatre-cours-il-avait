using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    [SerializeField] private float radius = 3f, agentColliderSize = 1.6f;

    [SerializeField] private bool showGizmo = true;

    // paramètre gizmo
    private float[] dangersResultTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        foreach (var obstacleCollider in aiData.obstacles)
        {
            var directionToObstacle
                = obstacleCollider.ClosestPoint(transform.position) - transform.position;
            var distanceToObstacle = directionToObstacle.magnitude;

            // Calcule le poids(weight) selon la distance Ennemi<--->Obstacle
            var weight= distanceToObstacle <= agentColliderSize ? 1 : (radius - distanceToObstacle) / radius;
            
            // Ajoute le paramètre obstacle dans le array danger
            for (var i = 0; i < Directions.eightDirections.Count; i++)
            {
                var result = Vector3.Dot(directionToObstacle.normalized, Directions.eightDirections[i]);

                var valueToPutIn = result * weight;

                // override la valeur seulement si elle est supérieure à la valeur dans le array danger
                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }
        dangersResultTemp = danger;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;

        if (Application.isPlaying && dangersResultTemp != null)
        {
            if (dangersResultTemp != null)
            {
                Gizmos.color = Color.red;
                for (var i = 0; i < dangersResultTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position,Directions.eightDirections[i] * dangersResultTemp[i] * 2);
                }
            }
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}

public static class Directions
{
    public static List<Vector3> eightDirections = new()
    {
        new Vector3(0,0,1).normalized,
        new Vector3(1,0,1).normalized, 
        new Vector3(1,0,0).normalized, 
        new Vector3(1,0,-1).normalized,
        new Vector3(0,0,-1).normalized, 
        new Vector3(-1,0,-1).normalized,
        new Vector3(-1,0,0).normalized,
        new Vector3(-1,0,1).normalized
    };
}
