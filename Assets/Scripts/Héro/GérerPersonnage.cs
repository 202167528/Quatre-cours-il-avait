using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GérerPersonnage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var nomObjet = collision.gameObject.name;
        if (nomObjet == "Wooden_DoorFin")
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
    public void PlayGame()
    {
        if (SceneManager.GetActiveScene().name == "Fond menu 2")
        {
            SceneManager.LoadScene("Tutoriel");
            life = 5;
        }
        else
        {
            int valeur = Random.Range(0, 2);

            if (valeur == 0)
                SceneManager.LoadScene("GenerationJulien");
            else
                SceneManager.LoadScene("Plateformes");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitter");
        Application.Quit();
    }

    public static GérerPersonnage Instance
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
        //currentScène;
    }

    // Data persisted between scenes
    public int life;
    //public string currentScène = "";
    //public float weapon = 0;


    private void Update()
    {
        life = PlayerStats.Instance.Health;
        if (life == 0)
        {
            SceneManager.LoadScene("Mort");
            life = 5;
        }

        //animation mort

    }
}
