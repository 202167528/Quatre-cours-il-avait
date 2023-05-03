using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTutorielTest : MonoBehaviour
{
    int vie;
    int layerD�gat = 9;
    [SerializeField] private LayerMask layerPorte;
    float radius = 1.5f;

    GameManagerD�but gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManagerD�but").GetComponent<GameManagerD�but>();
        vie = gameManager.vie;
    }

    //private void Update()
    //{
    //    var colliders = Physics.OverlapSphere(transform.position, radius, layerPorte);
    //    if(colliders.Length > 0)
    //    {
    //        gameManager.ChangerSc�ne();
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Wooden_DoorFin")
        {
            gameManager.ChangerSc�ne();
        }
        Debug.Log("Il y a eu contact avec " + collision.gameObject.name);

        if (collision.gameObject.layer == layerD�gat)
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

