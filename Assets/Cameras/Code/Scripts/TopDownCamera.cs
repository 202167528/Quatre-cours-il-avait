using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float height = 10;

    [SerializeField] private float distance = 10;

    [SerializeField] private float angle = 90;

    [SerializeField] private float smoothSpeed = 0.5f;

    private Vector3 refVector;
    
    // paramètre Gizmos
    [SerializeField] private float radius = 0.5f;
    
    private void Start()
    {
        HandleCamera();
    }

    private void Update()
    {
        HandleCamera();
    }

    protected virtual void HandleCamera()
    {
        if (!target)
            return;

        // Construit le vecteur worldPosition
        var worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);
        //Debug.DrawLine(target.position, worldPosition, Color.blue);
        
        // Construit le vecteur Rotation
        var rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
        //Debug.DrawLine(target.position, rotatedVector, Color.green);
        
        // Bouge notre position
        var flatTargetPosition = target.position;
        flatTargetPosition.y = 0f;
        var finalPosition = flatTargetPosition + rotatedVector;
        //Debug.DrawLine(target.position, finalPosition, Color.yellow);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVector, smoothSpeed);
        transform.LookAt(flatTargetPosition);
    }

    private void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }

        Gizmos.DrawSphere(transform.position, radius);
    }
}
