using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class AideStructure
{
    public static List<Noeuds> TraverserGraphePourExtraireFeuilleBasse(Noeuds parentNoeud)
    {
        Queue<Noeuds> noeudsÀVérifier = new Queue<Noeuds>();
        List<Noeuds> listePourRetourner = new List<Noeuds>();
        if (parentNoeud.ChildenListNoeuds.Count == 0)
        {
            return new List<Noeuds>() { parentNoeud };
        }
        foreach (var child in parentNoeud.ChildenListNoeuds)
        {
            noeudsÀVérifier.Enqueue(child);
        }
        while (noeudsÀVérifier.Count > 0)
        {
            var noeudCourant = noeudsÀVérifier.Dequeue();
            if (noeudCourant.ChildenListNoeuds.Count == 0)
            {
                listePourRetourner.Add(noeudCourant);
            }
            else
            {
                foreach (var child in noeudCourant.ChildenListNoeuds)
                {
                    noeudsÀVérifier.Enqueue(child);
                }
            }
        }
        return listePourRetourner;
    }

    public static Vector2Int GénérerCoinBasGaucheEntre(Vector2Int limitePointGauche, Vector2Int limitePointDroit, float modificateurPoint, int décalage)
    {
        int minX = limitePointGauche.x + décalage;
        int maxX = limitePointDroit.x - décalage;
        int minY = limitePointGauche.y + décalage;
        int maxY = limitePointDroit.y - décalage;
        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * modificateurPoint)),
            Random.Range(minY, (int)(minY + (minY - minY) * modificateurPoint)));
    }
    public static Vector2Int GénérerCoinHautDroitEntre(Vector2Int limitePointGauche, Vector2Int limitePointDroit, float modificateurPoint, int décalage)
    {
        int minX = limitePointGauche.x + décalage;
        int maxX = limitePointDroit.x - décalage;
        int minY = limitePointGauche.y + décalage;
        int maxY = limitePointDroit.y - décalage;
        return new Vector2Int(
            Random.Range((int)(minX+(maxX-minX)*modificateurPoint),maxX),
            Random.Range((int)(minY+(maxY-minY)*modificateurPoint),maxY));
    }

    public static Vector2Int CalculerPointMilieu(Vector2Int vec1, Vector2Int vec2)
    {
        Vector2 somme = vec1 + vec2;
        Vector2 tempVect = somme / 2;
        return new Vector2Int((int)tempVect.x,(int)tempVect.y);
    }
}
public enum PositionRelative { Haut, Bas, Droite, Gauche }