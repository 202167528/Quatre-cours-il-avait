using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RécupérationInformationTutoriel : MonoBehaviour
{
    public int vie;
    GameManagerDébut gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManagerDébut").GetComponent<GameManagerDébut>();
        vie = gameManager.vie;
    }

    public void PlayGame()
    {
        gameManager.ChangerScène();
    }
}


