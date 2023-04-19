using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cercle : MonoBehaviour
{
   [SerializeField] private float Rayon = 5f;
   
   private float AngleActuel = 0f;
   
    private void Update()
    {
        var angle = AngleActuel + Time.deltaTime;
        var positionX = Rayon * (Mathf.Cos(angle) - Mathf.Cos(AngleActuel));
        var positionZ = Rayon * (Mathf.Sin(angle) - Mathf.Sin(AngleActuel));
        
        transform.Translate(positionX, 0, positionZ);
        AngleActuel = angle;
    }
}