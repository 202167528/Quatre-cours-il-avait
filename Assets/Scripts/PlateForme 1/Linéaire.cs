using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linéaire : MonoBehaviour
{
    [SerializeField] float DuréeDeVie = 3f;

    float DéplacementObjet = 0.25f;

    float temps = 0f;

    void Update()
    {
        temps += Time.deltaTime;
        if (temps >= DéplacementObjet)
        {
            transform.Translate(0, 0, -0.05f);
            if (temps >= DuréeDeVie)
            {
                temps = 0f;
                Destroy(gameObject);
            }
        }
    }
}