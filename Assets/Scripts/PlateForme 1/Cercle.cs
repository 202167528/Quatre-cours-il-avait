using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cercle : MonoBehaviour
{
   [SerializeField] private float Rayon = 5f;

   [SerializeField] private float AngleActuel = 10f;

    private void FixedUpdate()
    {
        var angle = AngleActuel + Time.deltaTime;
        var positionX = Rayon * (Mathf.Cos(angle) - Mathf.Cos(AngleActuel));
        var positionZ = Rayon * (Mathf.Sin(angle) - Mathf.Sin(AngleActuel));

        transform.Translate(positionX, 0, positionZ);
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