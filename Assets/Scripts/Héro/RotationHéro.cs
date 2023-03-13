using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHéro : MonoBehaviour
{
    [SerializeField] GameObject joueur;
    [SerializeField] private float vitesseAngulaire;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        float souris = Input.GetAxis("Mouse X") * vitesseAngulaire;
        joueur.transform.eulerAngles = new Vector3(0, joueur.transform.eulerAngles.y + souris, 0);
    }
}
