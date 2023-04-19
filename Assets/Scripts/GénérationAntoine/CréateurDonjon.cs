using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CréateurDonjon : MonoBehaviour
{
    public int largeurDonjon, hauteurDonjon;
    public int pièceLargeurMin, pièceHauteurMin;
    public int itérationMax;
    public int largeurCouloir;
    public Material matériel;
    [Range(0.0f, 0.3f)]
    public float modificateurCoinBas;
    [Range(0.7f, 1.0f)]
    public float modificateurCoinHaut;
    [Range(0, 2)]
    public int décalage;
    public GameObject murVerticaleGauche, murHorizontaleBas, murVerticaleDroite, murHorizontaleHaut,
        murVerticaleGaucheTorche, murHorizontaleBasTorche, murVerticaleDroiteTorche, murHorizontaleHautTorche,
        murVerticaleGaucheBiblio, murHorizontaleBasBiblio, murVerticaleDroiteBiblio, murHorizontaleHautBiblio,
        murVerticaleGaucheÉtage, murHorizontaleBasÉtage, murVerticaleDroiteÉtage, murHorizontaleHautÉtage,
        murSortie, murEntrée, player;


    public GameObject baril, pierres1, pierres2, pierres3, champi1, champi2, champi3, tapis1, tapis2, épée2, roche, petitePotion, grossePotion;

    double[] probabilitésObjets;
    GameObject[] ObjetsPossibles;
    List<GameObject> objetsADétruire;
    List<GameObject> listePrefabObjets;
    List<Vector3Int> positionPortesPossiblesVerticale;
    List<Vector3Int> positionPortesPossiblesHorizontale;
    List<Vector3Int> positionMurPossibleVerticaleGauche;
    List<Vector3Int> positionMurPossibleHorizontaleBas;
    List<Vector3Int> positionMurPossibleVerticaleDroite;
    List<Vector3Int> positionMurPossibleHorizontaleHaut;
    List<Vector3Int> mursVerticaux;
    List<Vector3Int> mursHorizontaux;
    Vector3 MursGaucheLePlusLoin;
    Vector3 MursDroiteLePlusLoin;
    Vector3 PositionEntrée;
    Vector3 PositionSortie;
    [SerializeField]
    int probabilité;
    private void Awake()
    {
        CréereDonjon();
        InstantierHéro();
    }

    void Update()
    {
        DétruirePrefabExtérieur();
    }
    private void InstantierHéro()
    {
        var entrancePosition = new Vector3(PositionEntrée.x, PositionEntrée.y, PositionEntrée.z + 0.5f);
        var playerPosition = new Vector3(entrancePosition.x - 1, player.transform.position.y, entrancePosition.z);
        player.transform.position = playerPosition;
        //player.transform.forward = -(entrancePosition - playerPosition).normalized;
    }

    private void DétruirePrefabExtérieur()
    {
        for (int i = listePrefabObjets.Count - 1; i >= 0; i--)
        {
            var obj = listePrefabObjets[i];
            var objPosition = obj.transform.position;
            Vector3 position = new Vector3(objPosition.x, 0.01f, objPosition.z);
            Debug.DrawRay(position, Vector3.down, Color.red);
            if (!Physics.Raycast(position, Vector3.down))
            {
                Destroy(obj);
                listePrefabObjets.Remove(obj);
            }
        }
        List<GameObject> listeÀDétruire = new List<GameObject>();
        for (int j = 0; j < listePrefabObjets.Count; j++)
        {
            var X = listePrefabObjets[j].transform.position.x;
            var Xint = Mathf.FloorToInt(X);
            var Y = listePrefabObjets[j].transform.position.y;
            var Yint = Mathf.FloorToInt(Y);
            var Z = listePrefabObjets[j].transform.position.z;
            var Zint = Mathf.FloorToInt(Z);
            Vector3Int positionInt = new Vector3Int(Xint, Yint, Zint);
            if (mursVerticaux.Contains(positionInt) || mursHorizontaux.Contains(positionInt))
            {
                listeÀDétruire.Add(listePrefabObjets[j]);
                listePrefabObjets.Remove(listePrefabObjets[j]);
            }
        }
        for (int z = listeÀDétruire.Count-1; z >= 0; z--)
        {
            Destroy(listeÀDétruire[z]);

        }
    }

    private void CréereDonjon()
    {
        GénérateurDonjon générateur = new GénérateurDonjon(largeurDonjon, hauteurDonjon);
        var listeDePièces = générateur.CalculerDonjon(itérationMax, pièceLargeurMin, pièceHauteurMin, modificateurCoinBas, modificateurCoinHaut, décalage, largeurCouloir);
        GameObject mursParents = new GameObject("MursParents");
        mursParents.transform.parent = transform;
        positionPortesPossiblesVerticale = new List<Vector3Int>();
        positionPortesPossiblesHorizontale = new List<Vector3Int>();
        positionMurPossibleVerticaleDroite = new List<Vector3Int>();
        positionMurPossibleVerticaleGauche = new List<Vector3Int>();
        positionMurPossibleHorizontaleBas = new List<Vector3Int>();
        positionMurPossibleHorizontaleHaut = new List<Vector3Int>();
        mursVerticaux = new List<Vector3Int>();
        mursHorizontaux = new List<Vector3Int>();
        for (int i = 0; i < listeDePièces.Count; i++)
        {
            CréerMesh(listeDePièces[i].CoinBasGauche, listeDePièces[i].CoinHautDroit);
        }
        AjouterPrefab(listeDePièces);
        CréerMurs(mursParents);
    }

    private void AjouterPrefab(List<Noeuds> listeDePièces)
    {
        listePrefabObjets = new List<GameObject>();
        ObjetsPossibles = new GameObject[]
        { baril, pierres1, pierres2, pierres3, champi1, champi2, champi3, tapis1, tapis2, roche, épée2, petitePotion, grossePotion };
        probabilitésObjets = new double[] { 0, 0.03, 0.08, 0.12, 0.17, 0.2, 0.23, 0.26, 0.3, 0.33, 0.35, 0.37, 0.45, 1.1 };
        foreach (var pièce in listeDePièces)
        {
            if (!(pièce.CoinBasDroit.x - pièce.CoinBasGauche.x == largeurCouloir) && !(pièce.CoinHautGauche.y - pièce.CoinBasGauche.y == largeurCouloir))
            {
                var nbObjets = UnityEngine.Random.Range(0, (pièce.CoinBasDroit.x - pièce.CoinBasGauche.x) * (pièce.CoinHautGauche.y - pièce.CoinBasGauche.y) / 10);
                for (int i = 0; i < nbObjets; i++)
                {
                    var randomZ = UnityEngine.Random.Range(pièce.CoinHautGauche.y, pièce.CoinBasGauche.y);
                    var randomX = UnityEngine.Random.Range(pièce.CoinHautGauche.x, pièce.CoinHautDroit.x);
                    Vector3Int positionPrefab = new Vector3Int(randomX, 0, randomZ);
                    var probObj = UnityEngine.Random.Range(0f, 1f);
                    var indexObjetChoisi = -1;
                    for (int j = 0; j < probabilitésObjets.Length; j++)
                    {
                        if (probObj > probabilitésObjets[j] && probObj < probabilitésObjets[j + 1])
                        {
                            indexObjetChoisi = j;
                        }
                    }
                    var objet = ObjetsPossibles[indexObjetChoisi];
                    var objInstantié = Instantiate(objet, positionPrefab, Quaternion.identity);
                    listePrefabObjets.Add(objInstantié);
                }
            }
        }
    }

    private void CréerMurs(GameObject mursParents)
    {
        foreach (var positionMurs in positionMurPossibleHorizontaleBas)
        {
            var rand = UnityEngine.Random.Range(0, probabilité);
            if (rand <= 3)
            {
                CréerMur(mursParents, positionMurs, murHorizontaleBasTorche);
            }
            else if (rand == 5)
            {
                CréerMur(mursParents, positionMurs, murHorizontaleBasBiblio);
            }
            else if (rand == 6)
            {
                CréerMur(mursParents, positionMurs, murHorizontaleBasÉtage);
            }
            else
            {
                CréerMur(mursParents, positionMurs, murHorizontaleBas);
            }
        }
        foreach (var positionMurs in positionMurPossibleHorizontaleHaut)
        {
            var rand = UnityEngine.Random.Range(0, probabilité);
            if (rand <= 3)
            {
                CréerMur(mursParents, positionMurs, murHorizontaleHautTorche);
            }
            else if (rand == 5)
            {
                CréerMur(mursParents, positionMurs, murHorizontaleHautBiblio);
            }
            else if (rand == 6)
            {
                CréerMur(mursParents, positionMurs, murHorizontaleHautÉtage);
            }
            else
            {
                CréerMur(mursParents, positionMurs, murHorizontaleHaut);
            }
        }
        TrouverMursGaucheLePlusÉloigné();
        foreach (var positionMurs in positionMurPossibleVerticaleGauche)
        {
            if (positionMurs == PositionSortie)
            {
                CréerMur(mursParents, positionMurs, murSortie);
            }
            else
            {
                var rand = UnityEngine.Random.Range(0, probabilité);
                if (rand <= 2)
                {
                    CréerMur(mursParents, positionMurs, murVerticaleGaucheTorche);
                }
                if (rand == 5)
                {
                    CréerMur(mursParents, positionMurs, murVerticaleGaucheBiblio);
                }
                if (rand == 6)
                {
                    CréerMur(mursParents, positionMurs, murVerticaleGaucheÉtage);
                }
                else
                {
                    CréerMur(mursParents, positionMurs, murVerticaleGauche);
                }
            }      
        }
        TroucerMursDroiteLePlusÉloigné();
        foreach (var positionMurs in positionMurPossibleVerticaleDroite)
        {
            if (positionMurs == PositionEntrée)
            {
                CréerMur(mursParents, positionMurs, murEntrée);
            }
            else
            {
                var rand = UnityEngine.Random.Range(0, probabilité);
                if (rand <= 2)
                {
                    CréerMur(mursParents, positionMurs, murVerticaleDroiteTorche);
                }
                if (rand == 5)
                {
                    CréerMur(mursParents, positionMurs, murVerticaleDroiteBiblio);
                }
                if (rand == 6)
                {
                    CréerMur(mursParents, positionMurs, murVerticaleDroiteÉtage);
                }
                else
                {
                    CréerMur(mursParents, positionMurs, murVerticaleDroite);
                }
            }
        }
    }

    private void TroucerMursDroiteLePlusÉloigné()
    {
        MursDroiteLePlusLoin = positionMurPossibleVerticaleDroite[1];
        List<Vector3> mursLesPlusLoins = new List<Vector3>();
        List<int> listIndex = new List<int>();
        for (int i = positionMurPossibleVerticaleDroite.Count() - 1; i >= 0; i--)
        {
            if (positionMurPossibleVerticaleDroite[i].x > MursDroiteLePlusLoin.x)
            {
                MursDroiteLePlusLoin = positionMurPossibleVerticaleDroite[i];
            }
        }
        foreach (var murs in positionMurPossibleVerticaleDroite)
        {
            if (murs.x == MursDroiteLePlusLoin.x)
            {
                mursLesPlusLoins.Add(murs);
                listIndex.Add(positionMurPossibleVerticaleDroite.IndexOf(murs));
            }
        }
        var random = UnityEngine.Random.Range(0 + 1, listIndex.Count - 1);
        int indexFinale = listIndex[random];
        PositionEntrée = positionMurPossibleVerticaleDroite[indexFinale];
    }

    private void TrouverMursGaucheLePlusÉloigné()
    {
        MursGaucheLePlusLoin = positionMurPossibleVerticaleGauche[1];
        List<Vector3> mursLesPlusLoins = new List<Vector3>();
        List<int> listIndex = new List<int>();
        for (int i = positionMurPossibleVerticaleGauche.Count() - 1; i >= 0; i--)
        {
            if (positionMurPossibleVerticaleGauche[i].x < MursGaucheLePlusLoin.x)
            {
                MursGaucheLePlusLoin = positionMurPossibleVerticaleGauche[i];
            }
        }
        foreach (var murs in positionMurPossibleVerticaleGauche)
        {
            if (murs.x == MursGaucheLePlusLoin.x)
            {
                mursLesPlusLoins.Add(murs);
                listIndex.Add(positionMurPossibleVerticaleGauche.IndexOf(murs));
            }
        }
        var random = UnityEngine.Random.Range(0 + 1, listIndex.Count - 1);
        int indexFinale = listIndex[random];
        PositionSortie = positionMurPossibleVerticaleGauche[indexFinale];
    }

    private void CréerMur(GameObject mursParents, Vector3Int positionMurs, GameObject mursPrefab)
    {
        var mur = Instantiate(mursPrefab, positionMurs, Quaternion.identity, mursParents.transform);
        mur.layer = 3;
    }
    private void CréerMesh(Vector2 coinBasGauche, Vector2 coinHautDroit)
    {
        Vector3 sBasGauche = new Vector3(coinBasGauche.x, 0, coinBasGauche.y);
        Vector3 sBasDroit = new Vector3(coinHautDroit.x, 0, coinBasGauche.y);
        Vector3 sHautGauche = new Vector3(coinBasGauche.x, 0, coinHautDroit.y);
        Vector3 sHautDroit = new Vector3(coinHautDroit.x, 0, coinHautDroit.y);
        Vector3[] sommets = new Vector3[] { sHautGauche, sHautDroit, sBasGauche, sBasDroit };
        Vector2[] uvs = new Vector2[sommets.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(sommets[i].x, sommets[i].z);
        }
        int[] triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        Mesh mesh = new Mesh();
        mesh.vertices = sommets;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        GameObject soldonjon = new GameObject("Mesh" + coinBasGauche, typeof(MeshFilter), typeof(MeshRenderer));
        soldonjon.transform.position = Vector3.zero;
        soldonjon.transform.localScale = Vector3.one;
        soldonjon.GetComponent<MeshFilter>().mesh = mesh;
        soldonjon.GetComponent<MeshRenderer>().material = matériel;
        soldonjon.AddComponent<MeshCollider>();
        soldonjon.AddComponent<BoxCollider>();

        for (int rangée = (int)sBasGauche.x; rangée < (int)sBasDroit.x; rangée++)
        {
            var positionMurs = new Vector3(rangée, 0, sBasGauche.z);
            AjouterPositionMursÀListeHorizontale(positionMurs, positionMurPossibleHorizontaleBas, positionPortesPossiblesHorizontale);
        }
        for (int rangée = (int)sHautGauche.x; rangée < (int)sHautDroit.x; rangée++)
        {
            var positionMurs = new Vector3(rangée, 0, sHautDroit.z);
            AjouterPositionMursÀListeHorizontale(positionMurs, positionMurPossibleHorizontaleHaut, positionPortesPossiblesHorizontale);
        }
        CorrigerListMursHorizontaux(positionMurPossibleHorizontaleBas, positionMurPossibleHorizontaleHaut, positionPortesPossiblesHorizontale);
        for (int col = (int)sBasGauche.z; col < (int)sHautGauche.z; col++)
        {
            var positionMurs = new Vector3(sBasGauche.x, 0, col);
            AjouterPositionMursÀListeVerticale(positionMurs, positionMurPossibleVerticaleGauche, positionPortesPossiblesVerticale);
        }
        for (int col = (int)sBasDroit.z; col < (int)sHautDroit.z; col++)
        {
            var positionMurs = new Vector3(sBasDroit.x, 0, col);
            AjouterPositionMursÀListeVerticale(positionMurs, positionMurPossibleVerticaleDroite, positionPortesPossiblesVerticale);
        }
        CorrigerListMursVerticaux(positionMurPossibleVerticaleGauche, positionMurPossibleVerticaleDroite, positionPortesPossiblesVerticale);
    }
    private void CorrigerListMursVerticaux(List<Vector3Int> positionMurPossibleVerticaleGauche, List<Vector3Int> positionMurPossibleVerticaleDroite, List<Vector3Int> positionPortesPossiblesVerticale)
    {
        foreach (var pointPortes in positionPortesPossiblesVerticale)
        {
            positionMurPossibleVerticaleGauche.Remove(pointPortes);
        }
        foreach (var pointPortes in positionPortesPossiblesVerticale)
        {
            positionMurPossibleVerticaleDroite.Remove(pointPortes);
        }
    }

    private void CorrigerListMursHorizontaux(List<Vector3Int> positionMurPossibleHorizontaleBas, List<Vector3Int> positionMurPossibleHorizontaleHaut, List<Vector3Int> positionPortesPossiblesHorizontale)
    {
        foreach (var pointPortes in positionPortesPossiblesHorizontale)
        {
            positionMurPossibleHorizontaleBas.Remove(pointPortes);
        }
        foreach (var pointPortes in positionPortesPossiblesHorizontale)
        {
            positionMurPossibleHorizontaleHaut.Remove(pointPortes);
        }
    }

    private void AjouterPositionMursÀListeVerticale(Vector3 positionMurs, List<Vector3Int> listMurs, List<Vector3Int> listPortes)
    {
        Vector3Int point = Vector3Int.CeilToInt(positionMurs);
        if (mursVerticaux.Contains(point))
        {
            listPortes.Add(point);
            listMurs.Remove(point);
        }
        else
        {
            listMurs.Add(point);
        }
        mursVerticaux.Add(point);
    }
    private void AjouterPositionMursÀListeHorizontale(Vector3 positionMurs, List<Vector3Int> listMurs, List<Vector3Int> listPortes)
    {
        Vector3Int point = Vector3Int.CeilToInt(positionMurs);
        if (mursHorizontaux.Contains(point))
        {
            listPortes.Add(point);
            listMurs.Remove(point);
        }
        else
        {
            listMurs.Add(point);
        }
        mursHorizontaux.Add(point);
    }
}

