using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Noeuds
{
    private List<Noeuds> childenListNoeuds;

    public List<Noeuds> ChildenListNoeuds { get => childenListNoeuds; }

    public bool Visté { get; set; }
    public Vector2Int CoinBasGauche { get; set; }
    public Vector2Int CoinBasDroit { get; set; }
    public Vector2Int CoinHautDroit { get; set; }
    public Vector2Int CoinHautGauche { get; set; }
    public Noeuds Parent { get; set; }
    public int IndexArbre { get; set; }
    public Noeuds(Noeuds noeudParent)
    {
        childenListNoeuds = new List<Noeuds>();
        this.Parent = noeudParent;
        if (noeudParent != null)
        {
            noeudParent.AjouterChild(this);
        }
    }

    public void AjouterChild(Noeuds noeuds)
    {
        childenListNoeuds.Add(noeuds);
    }
    public void RetirerChild(Noeuds noeuds)
    {
        childenListNoeuds.Remove(noeuds);
    }
}