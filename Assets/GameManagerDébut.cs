using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerD�but : MonoBehaviour
{
    public int vie;
    public int numSc�ne = 0;

    public static GameManagerD�but instance;
    public string[] NomsSc�nes =
    { "Tutoriel", "SceneTutoriel", "GenerationJulien", "G�n�rationPro", "Plateformes", "G�n�rationPro", "Mort" };

    private static bool IsCreated;

    static GameManagerD�but()
    {
        IsCreated = false;
    }

    void Awake()
    {
        if (!IsCreated)
        {
            DontDestroyOnLoad(this.gameObject);
            IsCreated = true;
        }
    }
    public void DescendreVie()
    {
        --vie;
    }
    public void ChangerSc�ne()
    {
        ++numSc�ne;
        SceneManager.LoadScene(NomsSc�nes[numSc�ne]);
    }

    public void QuitGame()
    {
        Debug.Log("Quitter");
        Application.Quit();
    }
}
