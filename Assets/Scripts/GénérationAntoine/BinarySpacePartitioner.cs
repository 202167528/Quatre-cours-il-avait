using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

internal class BinarySpacePartitioner
{
    NoeudPièce racineNoeud;
    public NoeudPièce RacineNoeud { get => racineNoeud; }
    public BinarySpacePartitioner(int largeurDonjon, int hauteurDonjon)
    {
        this.racineNoeud = new NoeudPièce(new Vector2Int(0, 0), new Vector2Int(largeurDonjon, hauteurDonjon), null, 0);
    }
    public List<NoeudPièce> PréparerCollectionNoeuds(int itérationMax, int largeurMinPièce, int hauteurMinPièce)
    {
        Queue<NoeudPièce> graph = new Queue<NoeudPièce>();
        List<NoeudPièce> listÀretourner = new List<NoeudPièce>();
        graph.Enqueue(this.racineNoeud);
        listÀretourner.Add(this.racineNoeud);
        int itérations = 0;
        while (itérations < itérationMax && graph.Count > 0)
        {
            itérations++;
            NoeudPièce noeudCourant = graph.Dequeue();
            if (noeudCourant.Largeur >= largeurMinPièce * 2 || noeudCourant.Hauteur >= hauteurMinPièce * 2)
            {
                SéparerEspace(noeudCourant, listÀretourner, hauteurMinPièce, largeurMinPièce, graph);
            }

        }
        return listÀretourner;
    }

    private void SéparerEspace(NoeudPièce noeudCourant, List<NoeudPièce> listÀretourner, int hauteurMinPièce, int largeurMinPièce, Queue<NoeudPièce> graph)
    {
        Ligne ligne = ObtenirLigneDiviséEspace(noeudCourant.CoinBasGauche, noeudCourant.CoinHautDroit, largeurMinPièce, hauteurMinPièce);
        NoeudPièce noeud1, noeud2;
        if (ligne.Orientation == Orientation.Horizontale)
        {
            noeud1 = new NoeudPièce(noeudCourant.CoinBasGauche, new Vector2Int(noeudCourant.CoinHautDroit.x, ligne.Coordonées.y), noeudCourant, noeudCourant.IndexArbre + 1);
            noeud2 = new NoeudPièce(new Vector2Int(noeudCourant.CoinBasGauche.x, ligne.Coordonées.y), noeudCourant.CoinHautDroit, noeudCourant, noeudCourant.IndexArbre + 1);
        }
        else
        {
            noeud1 = new NoeudPièce(noeudCourant.CoinBasGauche, new Vector2Int(ligne.Coordonées.x, noeudCourant.CoinHautDroit.y), noeudCourant, noeudCourant.IndexArbre + 1);
            noeud2 = new NoeudPièce(new Vector2Int(ligne.Coordonées.x, noeudCourant.CoinBasGauche.y), noeudCourant.CoinHautDroit, noeudCourant, noeudCourant.IndexArbre + 1);
        }
        AjouterNouveauNoeudÀCollection(listÀretourner, graph, noeud1);
        AjouterNouveauNoeudÀCollection(listÀretourner, graph, noeud2);
    }

    private void AjouterNouveauNoeudÀCollection(List<NoeudPièce> listÀretourner, Queue<NoeudPièce> graph, NoeudPièce noeud)
    {
        listÀretourner.Add(noeud);
        graph.Enqueue(noeud);
    }

    private Ligne ObtenirLigneDiviséEspace(Vector2Int coinBasGauche, Vector2Int coinHautDroit, int largeurMinPièce, int hauteurMinPièce)
    {
        Orientation orientation;
        bool statuHauteur = (coinHautDroit.y - coinBasGauche.y) >= 2 * hauteurMinPièce;
        bool statuLargeur = (coinHautDroit.x - coinBasGauche.x) >= 2 * largeurMinPièce;
        if (statuHauteur && statuLargeur)
        {
            orientation = (Orientation)(Random.Range(0, 2));
        }
        else if (statuLargeur)
        {
            orientation = Orientation.Verticale;
        }
        else
        {
            orientation = Orientation.Horizontale;
        }
        return new Ligne(orientation, ObtenirCoordonésOrientation(orientation, coinBasGauche, coinHautDroit, largeurMinPièce,hauteurMinPièce));
    }

    private Vector2Int ObtenirCoordonésOrientation(Orientation orientation, Vector2Int coinBasGauche, Vector2Int coinHautDroit, int largeurMinPièce, int hauteurMinPièce)
    {
        Vector2Int coordonées = Vector2Int.zero;
        if (orientation == Orientation.Horizontale)
        {
            coordonées = new Vector2Int(
                0, Random.Range(
                (coinBasGauche.y + hauteurMinPièce),
                (coinHautDroit.y - hauteurMinPièce)));
        }
        else
        {
            coordonées = new Vector2Int(
                Random.Range(
                (coinBasGauche.x + largeurMinPièce),
                (coinHautDroit.x - largeurMinPièce)),0);
        }
        return coordonées;
    }
}