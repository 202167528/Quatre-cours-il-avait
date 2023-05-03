using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerDébut : MonoBehaviour
{
    public int vie;
    public int numScène = 0;

    public static GameManagerDébut instance;
    public string[] NomsScènes =
    { "Tutoriel", "SceneTutoriel", "GenerationJulien", "GénérationPro", "Plateformes", "GénérationPro", "Mort" };

    private static bool IsCreated;

    static GameManagerDébut()
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
    public void ChangerScène()
    {
        ++numScène;
        SceneManager.LoadScene(NomsScènes[numScène]);
    }

    public void QuitGame()
    {
        Debug.Log("Quitter");
        Application.Quit();
    }
}
