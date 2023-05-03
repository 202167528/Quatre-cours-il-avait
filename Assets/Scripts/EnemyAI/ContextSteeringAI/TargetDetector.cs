using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField] private float targetDetectionRange = 5f;

    [SerializeField] private float targetDetectionAngle = 150;
    
    [SerializeField] private LayerMask obstaclesLayerMask, playerLayerMask;

    [SerializeField] private bool showGizmos;

    // paramètre gizmo
    private List<Transform> colliders;

    public override void Detect(AIData aiData)
    {
        // Trouve si le joueur est proche
        var playerCollider = Physics.OverlapSphere(transform.position, targetDetectionRange, playerLayerMask);

        if (playerCollider.Length > 0)
        {
            var direction = (playerCollider[0].transform.position - transform.position).normalized;
            
            // S'assure que le collider du joueur est dans son champ de vision
            if (Vector3.Angle(transform.forward, direction) < targetDetectionAngle / 2)
            {
                if (Physics.Raycast(transform.position, direction, out var hit, targetDetectionRange,
                        obstaclesLayerMask))
                {
                    // S'assure que le collider du joueur qu'il voit est sur le layer "Player"
                    if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        Debug.DrawRay(transform.position, direction * Vector3.Distance(playerCollider[0].transform.position, transform.position), Color.magenta);
                        colliders = new List<Transform>() { playerCollider[0].transform };
                    }
                    else
                    {
                        // L'ennemi voit un obstacle, mais pas le joueur
                        colliders = null;
                    }
                }
                else
                {
                    // L"ennemi ne voit aucun collider
                    colliders = null;
                }
            }
            else
            {
                // Le joueur n'est pas dans le champ de vision de l'ennemi
                colliders = null;
            }
        }
        else
        {
            // L'ennemi ne voit pas le joueur
            colliders = null;
        }
        aiData.targets = colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos == false)
            return;

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}
