using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTutorielTest : MonoBehaviour
{
    int vie;
    int layerDégat = 9;

    GameManagerDébut gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManagerDébut").GetComponent<GameManagerDébut>();
        vie = gameManager.vie;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Wooden_DoorFin")
        {
            gameManager.ChangerScène();
        }
        Debug.Log("Il y a eu contact avec "+ collision.gameObject.name);
        
        if (collision.gameObject.layer == layerDégat)
        {
            Debug.Log("Collision du layer");
            HealthBarHUDTester viePerso = gameObject.AddComponent<HealthBarHUDTester>();
            viePerso.Hurt(1);
            gameManager.DescendreVie();
        }

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Un message");
    }
    public void RetournerMenue()
    {
        SceneManager.LoadScene("Fond menu 2");
    }
}

