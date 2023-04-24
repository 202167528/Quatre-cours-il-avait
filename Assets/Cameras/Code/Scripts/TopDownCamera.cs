using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float height = 10;

    [SerializeField] private float distance = 10;

    [SerializeField] private float angle = 90;

    [SerializeField] private float smoothSpeed = 0.5f;

    [SerializeField] private LayerMask layermask;

    [SerializeField] private Material matOpaque;

    [SerializeField] private Material matTransparent;

    private Vector3 refVector;

    List<MeshRenderer> renderers = new List<MeshRenderer>();

    bool derrièreMure = false;

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
        EnleverRenderer();

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
        //if (target)
        //{
        //    Gizmos.DrawLine(transform.position, target.position);
        //}

        //Gizmos.DrawSphere(transform.position, radius);
    }
    private void EnleverRenderer()
    {
        foreach (var murs in renderers)
        {
            murs.material = matOpaque;
        }
        renderers.Clear();
        derrièreMure = false;
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 directionAvant1 = new Vector3(direction.x, direction.y, direction.z + 0.1f);
        Vector3 directionAvant2 = new Vector3(direction.x, direction.y, direction.z + 0.2f);
        Vector3 directionArrière1 = new Vector3(direction.x, direction.y, direction.z - 0.1f);
        Vector3 directionArrière2 = new Vector3(direction.x, direction.y, direction.z - 0.2f);
        float distance = Vector3.Distance(transform.position, target.position);
        if (Physics.Raycast(transform.position, direction * distance, out RaycastHit hit, Mathf.Infinity, layermask))
        {
            var mesh = hit.collider.GetComponent<MeshRenderer>();
            renderers.Add(mesh);
            mesh.material = matTransparent;
            derrièreMure = true;
        }
        if (Physics.Raycast(transform.position, directionAvant1 * (distance + 1f), out RaycastHit hitAv1, Mathf.Infinity, layermask) && derrièreMure == true)
        {
            var mesh = hitAv1.collider.GetComponent<MeshRenderer>();
            renderers.Add(mesh);
            mesh.material = matTransparent;
        }
        if (Physics.Raycast(transform.position, directionAvant2 * (distance + 2f), out RaycastHit hitAv2, Mathf.Infinity, layermask) && derrièreMure == true)
        {
            var mesh = hitAv2.collider.GetComponent<MeshRenderer>();
            renderers.Add(mesh);
            mesh.material = matTransparent;
        }
        if (Physics.Raycast(transform.position, directionArrière1 * (distance + 1f), out RaycastHit hitAr1, Mathf.Infinity, layermask) && derrièreMure == true)
        {
            var mesh = hitAr1.collider.GetComponent<MeshRenderer>();
            renderers.Add(mesh);
            mesh.material = matTransparent;
        }
        if (Physics.Raycast(transform.position, directionArrière2 * (distance + 2f), out RaycastHit hitAr2, Mathf.Infinity, layermask) && derrièreMure == true)
        {
            var mesh = hitAr2.collider.GetComponent<MeshRenderer>();
            renderers.Add(mesh);
            mesh.material = matTransparent;
        }
        Debug.DrawRay(transform.position, direction * distance, Color.red);
    }
}
