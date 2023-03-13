using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaméraPerso : MonoBehaviour
{
    private GameObject joueur;
    [SerializeField] private float VitesseAngulaire = 3f;

    private Vector3 offset;
    private Vector3 finalOffset;



    private void Start()
    {  
        joueur = GameObject.Find("Head_M");
        var déplacement = joueur.transform.position - transform.position;
        transform.Translate(déplacement.x - 4, déplacement.y + 2, déplacement.z, Space.World);
        offset = transform.position - joueur.transform.position;
        finalOffset = offset;
    }


    private void Update()
    {
        Rotate();
        transform.position = Vector3.Lerp(transform.position, joueur.transform.position + finalOffset, 1.25f);
        transform.LookAt(joueur.transform.position);
    }

    void Rotate() =>
        finalOffset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * VitesseAngulaire, Vector3.up) * finalOffset;
    
}
