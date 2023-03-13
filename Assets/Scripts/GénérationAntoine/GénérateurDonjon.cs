using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GénérateurDonjon
{
    List<NoeudPièce> touteEspaceNoeuds = new List<NoeudPièce>();
    private int largeurDonjon;
    private int hauteurDonjon;

    public GénérateurDonjon(int largeurDonjon, int hauteurDonjon)
    {
        this.largeurDonjon = largeurDonjon;
        this.hauteurDonjon = hauteurDonjon;
    }

    public List<Noeuds> CalculerDonjon(int itérationMax, int pièceLargeurMin, int pièceHauteurMin, float modificateurCoinBas, float modificateurCoinHaut, int décalage, int largeurCouloir)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(largeurDonjon, hauteurDonjon);
        touteEspaceNoeuds = bsp.PréparerCollectionNoeuds(itérationMax, pièceLargeurMin, pièceHauteurMin);
        List<Noeuds> espacePièces = AideStructure.TraverserGraphePourExtraireFeuilleBasse(bsp.RacineNoeud);

        GégérateurPièce gégérateurPièce = new GégérateurPièce(itérationMax, pièceHauteurMin, pièceLargeurMin);
        List<NoeudPièce> listePièce = gégérateurPièce.GénérerPièceDansEspaceDonné(espacePièces, modificateurCoinBas, modificateurCoinHaut, décalage);

        GénérateurCouloir générateurCouloir = new GénérateurCouloir();
        var listeCouloir = générateurCouloir.CréerCouloir(touteEspaceNoeuds, largeurCouloir);
        return new List<Noeuds>(listePièce).Concat(listeCouloir).ToList();
    }
}