using System;
using System.Collections.Generic;
using UnityEngine;

public class GégérateurPièce
{
    private int itérationMax;
    private int pièceHauteurMin;
    private int pièceLargeurMin;

    public GégérateurPièce(int itérationMax, int pièceHauteurMin, int pièceLargeurMin)
    {
        this.itérationMax = itérationMax;
        this.pièceHauteurMin = pièceHauteurMin;
        this.pièceLargeurMin = pièceLargeurMin;
    }

    public List<NoeudPièce> GénérerPièceDansEspaceDonné(List<Noeuds> espacePièces, float modificateurCoinBas, float modificateurCoinHaut, int décalage)
    {
        List<NoeudPièce> listeÀRetourner = new List<NoeudPièce>();
        foreach (var espace in espacePièces)
        {
            Vector2Int nouveauCoinBasGauche = AideStructure.GénérerCoinBasGaucheEntre(espace.CoinBasGauche, espace.CoinHautDroit,modificateurCoinBas, décalage);
            Vector2Int nouveauCoinHautDroit = AideStructure.GénérerCoinHautDroitEntre(espace.CoinBasGauche, espace.CoinHautDroit, modificateurCoinHaut, décalage);
            espace.CoinBasGauche = nouveauCoinBasGauche;
            espace.CoinHautDroit = nouveauCoinHautDroit;
            espace.CoinBasDroit = new Vector2Int(nouveauCoinHautDroit.x, nouveauCoinBasGauche.y);
            espace.CoinHautGauche = new Vector2Int(nouveauCoinBasGauche.x, nouveauCoinHautDroit.y);
            listeÀRetourner.Add((NoeudPièce)espace);
        }
        return listeÀRetourner;
    }
}