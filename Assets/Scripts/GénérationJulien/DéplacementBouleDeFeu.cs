using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DéplacementBouleDeFeu : MonoBehaviour
{
    [SerializeField] int vitesse;
    Vector3 DéplacementPositif;
    int retour;
    void Start()
    {
        DéplacementPositif = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 0.8f)
        {
            retour = 1;
        }
        if (transform.position.x >= 43f)
        {
            retour = -1;
        }
        transform.Translate(retour * DéplacementPositif * vitesse * Time.deltaTime);
    }
}
