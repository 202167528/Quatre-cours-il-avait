using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class R�cup�rationInformationTutoriel : MonoBehaviour
{
    public int vie;
    GameManagerD�but gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManagerD�but").GetComponent<GameManagerD�but>();
        vie = gameManager.vie;
    }

    public void PlayGame()
    {
        gameManager.ChangerSc�ne();
    }
}


