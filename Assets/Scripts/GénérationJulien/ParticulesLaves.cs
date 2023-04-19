using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulesLaves : MonoBehaviour
{
    [SerializeField]
    int chiffreAléatoire;
    [SerializeField]
    int nombrePossibilités;
    ParticleSystem particules;
    private void Start()
    {
        particules = GetComponentInChildren<ParticleSystem>();
        particules.Stop();
    }
    void Update()
    {
        int chiffre = UnityEngine.Random.Range(0, nombrePossibilités);
        if (chiffre == chiffreAléatoire)
        {
            particules.Play();
        }
    }
}
