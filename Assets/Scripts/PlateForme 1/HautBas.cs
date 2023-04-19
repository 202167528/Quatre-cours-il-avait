using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HautBas : MonoBehaviour
{
    [SerializeField] private float Rayon = 5f;
   
    private float AngleActuel = 0f;
   
    private void Update()
    {
        var angle = AngleActuel + Time.deltaTime;
        var positionZ = Rayon * (Mathf.Sin(angle) - Mathf.Sin(AngleActuel));
        
        transform.Translate(0, 0, positionZ);
        AngleActuel = angle;
    }
}
