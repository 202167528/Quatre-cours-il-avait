using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ÉliminerPersonnage : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "HumanMale_Character_FREE")
        {
            HealthBarHUDTester viePerso = gameObject.AddComponent<HealthBarHUDTester>();
            viePerso.Hurt(1);
            Debug.Log("Vous avez pris un dommage");
        }
    }
}
