using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaméraPerso : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private float vitesseAngulaire = 3f;

    private Vector3 offset;
    private Vector3 finalOffset;
    private  Vector3 headPosition;

    private void Start()
    {
        headPosition = head.transform.position;
        var direction = (headPosition - transform.position).normalized;
        
        transform.Translate(direction.x * 4, direction.y * 2, direction.z, Space.World);
        offset = transform.position - head.transform.position;
        finalOffset = offset;
    }

    private void Update()
    {
        Rotate();
        transform.position = Vector3.Lerp(transform.position, head.transform.position + finalOffset, 1.25f);
        transform.LookAt(headPosition);
    }

    void Rotate() =>
        finalOffset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * vitesseAngulaire, Vector3.up) * finalOffset;
    
}
