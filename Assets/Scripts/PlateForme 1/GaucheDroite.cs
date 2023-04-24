using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaucheDroite : MonoBehaviour
{
    [SerializeField] private float Rayon = 5f;

    [SerializeField] private float AngleActuel = 0f;
   
    private void FixedUpdate()
    {
        var angle = AngleActuel + Time.deltaTime;
        var positionX = Rayon * (Mathf.Cos(angle) - Mathf.Cos(AngleActuel));
        
        transform.Translate(positionX, 0, 0);
        AngleActuel = angle;
    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
