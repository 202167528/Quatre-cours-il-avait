using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulesLaves : MonoBehaviour
{
    [SerializeField]
    int chiffreAl�atoire;
    [SerializeField]
    int nombrePossibilit�s;
    ParticleSystem particules;
    private void Start()
    {
        particules = GetComponentInChildren<ParticleSystem>();
        particules.Stop();
    }
    void Update()
    {
        int chiffre = UnityEngine.Random.Range(0, nombrePossibilit�s);
        if (chiffre == chiffreAl�atoire)
        {
            particules.Play();
        }
    }
}
