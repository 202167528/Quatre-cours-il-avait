using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipale : MonoBehaviour
{
    public void PlayGame()
    {
        if (SceneManager.GetActiveScene().name == "Fond menu 2")
            SceneManager.LoadScene("Tutoriel");
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
}