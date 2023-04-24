using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ChangerScène : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var nomObjet = collision.gameObject.name;
        if (nomObjet == "Wooden_Door")
        {
            if (SceneManager.GetActiveScene().name == "GénérationPro")
            {
                int valeur = Random.Range(0, 2);

                if (valeur == 0)
                    SceneManager.LoadScene("GenerationJulien");
                else
                    SceneManager.LoadScene("Plateformes");
            }
            else
                SceneManager.LoadScene("GénérationPro");
        }

        if (nomObjet == "Sol(Clone)" || nomObjet == "Plane")
        {
            HealthBarHUDTester viePerso = gameObject.AddComponent<HealthBarHUDTester>();
            viePerso.Hurt(1);
        }
    }
    public static ChangerScène Instance
    {
        get;
        set;
    }

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }

    void Start()
    {
        //Load first game scene (probably main menu)
        //Application.LoadLevel(2);
    }


    // Data persisted between scenes
    public int life;
    //public float armor = 0;
    //public float weapon = 0;


    private void Update()
    {
        life = PlayerStats.Instance.Health;
        if (life == 0)
        {
            SceneManager.LoadScene("Fond menu 2");
            life = 5;
        }
        
            //animation mort

    }
}
