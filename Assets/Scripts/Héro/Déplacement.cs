using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class Déplacement : MonoBehaviour
{
    [SerializeField] private float VitesseMarche;
    [SerializeField] private float VitesseSprint;
    [SerializeField] private float gravité;
    [SerializeField] private float hauteurSaut;
    private Vector3 direction;
    private Vector3 vitesse;
    private CharacterController controlleur;
    private Animator anim;

    private void Start()
    {
        controlleur = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {   
        Déplacer();
    }
    private void Déplacer()
    {
        float déplacementZ = Input.GetAxis("Vertical");
        float déplacementX = Input.GetAxis("Horizontal");
        direction = new Vector3(déplacementX, 0, déplacementZ);
        if (direction != Vector3.zero)
        {
            direction = direction * VitesseMarche;
            anim.SetFloat("z", déplacementZ);
            anim.SetFloat("x", déplacementX);
        }
        if (Input.GetKey(KeyCode.Space) && controlleur.isGrounded)
        {
            Sauter();

        }
        direction = transform.TransformDirection(direction);
        controlleur.Move(direction * Time.deltaTime);
        vitesse.y += gravité * Time.deltaTime;
        controlleur.Move(vitesse * Time.deltaTime);
    }
    private void Sauter()
    {
        anim.SetTrigger("Sauter");
        vitesse.y = Mathf.Sqrt(hauteurSaut * -2 * gravité);
        
    }
}