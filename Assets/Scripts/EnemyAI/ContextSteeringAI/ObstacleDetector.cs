using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : Detector
{
    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private float detectionRadius = 3;

    [SerializeField] private bool showGizmos = true;

    private Collider[] colliders;

    public override void Detect(AIData aiData)
    {
        colliders = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);
        aiData.obstacles = colliders;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;
        
        if (Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.red;
            foreach (var obstacleCollider in colliders)
            {
                Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
            }
        }
    }
}
