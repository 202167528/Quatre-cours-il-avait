using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrôleRammasage : MonoBehaviour
{
    Animator anim;
    public GameObject MainDroite;
    private GameObject leTrucDansMain;

    public GameObject Épée2Personnage;
    public GameObject Épée2Lancé;

    List<GameObject> ArmesPossible;
    List<GameObject> ArmesLancé;

    GameObject objetÀPrendre;
    GameObject objetÀLancé;

    bool procheArme;
    bool estDansLaMain;

    string TagCetObjet;
    string nomObjetÀPrendre;
    string nomObjetÀLancer;

    private Vector3 directionLancé;
    private Camera cam;
    public GameObject objetTrail;

    private float tempsÉcoulé = 2f;
    private void Start()
    {
        directionLancé = new Vector3(0, 0, 1);

        procheArme = false;
        estDansLaMain = false;

        ArmesPossible = new List<GameObject>();
        ArmesLancé = new List<GameObject>();

        cam = FindObjectOfType<Camera>();

        ArmesPossible.Add(Épée2Personnage);
        ArmesLancé.Add(Épée2Lancé);
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (procheArme)
        {
            nomObjetÀPrendre = TagCetObjet + "Personnage";
            objetÀPrendre = TrouverArmeDansListe();
            if (Input.GetKeyDown("f") && !estDansLaMain)
            {
                anim.SetTrigger("Ramasser");
                leTrucDansMain = Instantiate(objetÀPrendre, MainDroite.transform);
                estDansLaMain = true;
            }
            procheArme = false;
        }

        if (!(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("space")))
        {
            if ((Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) && estDansLaMain)
            {
                tempsÉcoulé += Time.deltaTime;
                nomObjetÀLancer = TagCetObjet + "Lancé";
                objetÀLancé = TrouverArmeLancéDansListe();

                if (tempsÉcoulé >= 2f)
                {
                    Ray r = new Ray(MainDroite.transform.position, MainDroite.transform.forward * -1);

                    Vector3 dir = r.GetPoint(1) - r.GetPoint(0);

                    GameObject bullet = Instantiate(objetTrail, r.GetPoint(2), Quaternion.LookRotation(dir));

                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;
                    tempsÉcoulé -= 2f;
                }
            }


            if (Input.GetMouseButtonUp(0) && estDansLaMain)
            {
                Ray r = new Ray(MainDroite.transform.position, MainDroite.transform.forward * -1);

                Vector3 dir = r.GetPoint(1) - r.GetPoint(0);

                GameObject bullet = Instantiate(objetÀLancé, r.GetPoint(2), Quaternion.LookRotation(dir));

                Destroy(leTrucDansMain);

                estDansLaMain = false;

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;
            }
        }
    }

    private GameObject TrouverArmeLancéDansListe()
    {
        for (int i = 0; i < ArmesLancé.Count; i++)
            if (ArmesLancé[i].name == nomObjetÀLancer)
                return ArmesLancé[i];

        return null;
    }

    private GameObject TrouverArmeDansListe()
    {
        for (int i = 0; i < ArmesPossible.Count; i++)
            if (ArmesPossible[i].name == nomObjetÀPrendre)
                return ArmesPossible[i];

        return null;
    }
    private void OnTriggerStay(Collider autre)
    {
        procheArme = true;
        TagCetObjet = autre.gameObject.tag;
        if (Input.GetKeyDown("f") && !estDansLaMain)
        {
            Destroy(autre.gameObject);
        }
    }
    private void OnTriggerExit(Collider autre)
    {
        procheArme = false;
    }
}
