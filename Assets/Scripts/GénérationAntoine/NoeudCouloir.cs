using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

internal class NoeudCouloir : Noeuds
{
    private Noeuds structure1;
    private Noeuds structure2;
    private int largeurCouloir;
    private int modificateurDistanceDuMur = 1;

    public NoeudCouloir(Noeuds noeuds1, Noeuds noeuds2, int largeurCouloir) : base(null)
    {
        this.structure1 = noeuds1;
        this.structure2 = noeuds2;
        this.largeurCouloir = largeurCouloir;
        GénérerCouloir();
    }

    private void GénérerCouloir()
    {
        var positionRelativeStructure2 = CheckPositionStructure2ContreStructure1();
        switch (positionRelativeStructure2)
        {
            case PositionRelative.Haut:
                ProcéderSaleRelationHautOuBas(this.structure1, this.structure2);
                break;
            case PositionRelative.Bas:
                ProcéderSaleRelationHautOuBas(this.structure2, this.structure1);
                break;
            case PositionRelative.Droite:
                ProcéderSaleRelationDroiteOuGauche(this.structure1,this.structure2);
                break;
            case PositionRelative.Gauche:
                ProcéderSaleRelationDroiteOuGauche(this.structure2, this.structure1);
                break;
            default:
                break;

        }
    }

    private void ProcéderSaleRelationDroiteOuGauche(Noeuds structure1, Noeuds structure2)
    {
        Noeuds structureGauche = null;
        List<Noeuds> childrenStructureGauche = AideStructure.TraverserGraphePourExtraireFeuilleBasse(structure1);
        Noeuds structureDroite = null;
        List<Noeuds> childrenStructureDroite = AideStructure.TraverserGraphePourExtraireFeuilleBasse(structure2);

        var structureGaucheTriés = childrenStructureGauche.OrderByDescending(child => child.CoinHautDroit.x).ToList();
        if (structureGaucheTriés.Count == 1)
        {
            structureGauche = structureGaucheTriés[0];
        }
        else
        {
            int maxX = structureGaucheTriés[0].CoinHautDroit.x;
            structureGaucheTriés = structureGaucheTriés.Where(children => Math.Abs(maxX - children.CoinHautDroit.x) < 10).ToList();
            int index = UnityEngine.Random.Range(0, structureGaucheTriés.Count);
            structureGauche = structureGaucheTriés[index];
        }

        var VoisinPossibleStructureDroite = childrenStructureDroite.Where(
            child => AvoirYValidePourVoisinGaucheDroite(structureGauche.CoinHautDroit, structureGauche.CoinBasDroit, child.CoinHautGauche, child.CoinBasGauche) != -1).OrderBy(child => child.CoinBasDroit.x).ToList();
        if (VoisinPossibleStructureDroite.Count <= 0)
        {
            structureDroite = structure2;
        }
        else
        {
            structureDroite = VoisinPossibleStructureDroite[0];
        }
        int y = AvoirYValidePourVoisinGaucheDroite(structureGauche.CoinHautGauche, structureGauche.CoinBasDroit, structureDroite.CoinHautGauche, structureDroite.CoinBasGauche);
        while (y == -1 && structureGaucheTriés.Count > 0)
        {
            structureGaucheTriés = structureGaucheTriés.Where(child => child.CoinHautGauche.y != structureGauche.CoinHautGauche.y).ToList();
            structureGauche = structureGaucheTriés[0];
            y = AvoirYValidePourVoisinGaucheDroite(structureGauche.CoinHautGauche, structureGauche.CoinBasDroit, structureDroite.CoinHautGauche, structureDroite.CoinBasGauche);
        }
        CoinBasGauche = new Vector2Int(structureGauche.CoinBasDroit.x, y);
        CoinHautDroit = new Vector2Int(structureDroite.CoinHautGauche.x, y + this.largeurCouloir);
    }
    private int AvoirYValidePourVoisinGaucheDroite(Vector2Int noeudHautGauche, Vector2Int noeudBasGauche, Vector2Int noeudHautDroit, Vector2Int noeudBasDroit)
    {
        if (noeudHautDroit.y >= noeudHautGauche.y && noeudBasGauche.y >= noeudBasDroit.y)
        {
            return AideStructure.CalculerPointMilieu(noeudBasGauche + new Vector2Int(0, modificateurDistanceDuMur),
                noeudHautGauche - new Vector2Int(0, modificateurDistanceDuMur + this.largeurCouloir)).y;
        }
        if (noeudHautDroit.y <= noeudHautGauche.y && noeudBasGauche.y <= noeudBasDroit.y)
        {
            return AideStructure.CalculerPointMilieu(noeudBasDroit + new Vector2Int(0,modificateurDistanceDuMur),
                noeudHautDroit - new Vector2Int(0, modificateurDistanceDuMur + this.largeurCouloir)).y;
        }
        if (noeudHautGauche.y >= noeudBasDroit.y && noeudHautGauche.y <= noeudHautDroit.y)
        {
            return AideStructure.CalculerPointMilieu(noeudBasDroit + new Vector2Int(0, modificateurDistanceDuMur),
                noeudHautGauche - new Vector2Int(0, modificateurDistanceDuMur + this.largeurCouloir)).y;
        }
        if (noeudBasGauche.y >= noeudBasDroit.y && noeudBasGauche.y <= noeudHautDroit.y)
        {
            return AideStructure.CalculerPointMilieu(noeudBasGauche + new Vector2Int(0, modificateurDistanceDuMur),
                noeudHautDroit - new Vector2Int(0, modificateurDistanceDuMur + this.largeurCouloir)).y;
        }
        return -1;
    }
    private void ProcéderSaleRelationHautOuBas(Noeuds structure1, Noeuds structure2)
    {
        Noeuds structureBas = null;
        List<Noeuds> structureBasChildren = AideStructure.TraverserGraphePourExtraireFeuilleBasse(structure1);
        Noeuds structureHaut = null;
        List<Noeuds> structureHautChildren = AideStructure.TraverserGraphePourExtraireFeuilleBasse(structure2);
        var structureBasTriés = structureBasChildren.OrderByDescending(child => child.CoinHautDroit.y).ToList();
        if (structureBasTriés.Count == 1)
        {
            structureBas = structureBasChildren[0];
        }
        else
        {
            int maxY = structureBasTriés[0].CoinHautGauche.y;
            structureBasTriés = structureBasTriés.Where(child => Mathf.Abs(maxY - child.CoinHautGauche.y) < 10).ToList();
            int index = UnityEngine.Random.Range(0, structureBasTriés.Count);
            structureBas = structureBasTriés[index];
        }

        var voisinPossibleStructureHaut = structureHautChildren.Where(child => ObtenirXValidePourVoisinHautOuBas(structureBas.CoinHautGauche,
            structureBas.CoinHautDroit, child.CoinBasGauche, child.CoinBasDroit) != -1).OrderBy(child => child.CoinBasDroit.y).ToList();
        if (voisinPossibleStructureHaut.Count == 0)
        {
            structureHaut = structure2;
        }
        else
        {
            structureHaut = voisinPossibleStructureHaut[0];
        }
        int x = ObtenirXValidePourVoisinHautOuBas(structureBas.CoinHautGauche,
        structureBas.CoinHautDroit, structureHaut.CoinBasGauche, structureHaut.CoinBasDroit);
        while (x == -1 && structureBasTriés.Count > 1)
        {
            structureBasTriés = structureBasTriés.Where(child => child.CoinHautGauche.x != structureHaut.CoinHautGauche.x).ToList();
            structureBas = structureBasTriés[0];
            x = ObtenirXValidePourVoisinHautOuBas(structureBas.CoinHautGauche,
               structureBas.CoinHautDroit, structureHaut.CoinBasGauche, structureHaut.CoinBasDroit);
        }
        CoinBasGauche = new Vector2Int(x, structureBas.CoinHautGauche.y);
        CoinHautDroit = new Vector2Int(x+this.largeurCouloir, structureHaut.CoinBasGauche.y);
    }

    private int ObtenirXValidePourVoisinHautOuBas(Vector2Int noeudHautGauche, Vector2Int noeudHautDroit, Vector2Int noeudBasGauche, Vector2Int noeudBasDroit)
    {
        if (noeudHautGauche.x < noeudBasGauche.x && noeudBasDroit.x < noeudHautDroit.x)
        {
            return AideStructure.CalculerPointMilieu(noeudBasGauche + new Vector2Int(modificateurDistanceDuMur, 0),
                noeudBasDroit - new Vector2Int(this.largeurCouloir + modificateurDistanceDuMur,0)).x;
        }
        if (noeudHautGauche.x >= noeudBasGauche.x && noeudBasDroit.x >= noeudHautDroit.x)
        {
            return AideStructure.CalculerPointMilieu(noeudHautGauche + new Vector2Int(modificateurDistanceDuMur, 0),
                noeudHautDroit - new Vector2Int(this.largeurCouloir + modificateurDistanceDuMur, 0)).x;
        }
        if (noeudBasGauche.x >= noeudHautGauche.x && noeudBasGauche.x <= noeudHautDroit.x)
        {
            return AideStructure.CalculerPointMilieu(noeudBasGauche + new Vector2Int(modificateurDistanceDuMur, 0),
                noeudHautDroit - new Vector2Int(this.largeurCouloir + modificateurDistanceDuMur, 0)).x;
        }
        if (noeudBasDroit.x <= noeudHautDroit.x && noeudBasDroit.x >= noeudHautGauche.x)
        {
            return AideStructure.CalculerPointMilieu(noeudHautGauche + new Vector2Int(modificateurDistanceDuMur, 0),
                noeudBasDroit - new Vector2Int(this.largeurCouloir + modificateurDistanceDuMur, 0)).x;
        }
        return -1;
    }

    private PositionRelative CheckPositionStructure2ContreStructure1()
    {
        Vector2 pointMillieuStructure1Temp = ((Vector2)structure1.CoinHautDroit + CoinBasGauche) / 2;
        Vector2 pointMillieuStructure2Temp = ((Vector2)structure2.CoinHautDroit + CoinBasGauche) / 2;
        float angle = CalculerAngle(pointMillieuStructure1Temp, pointMillieuStructure2Temp);
        if (angle < 45 && angle >= 0 || angle > -45 && angle < 0)
        {
            return PositionRelative.Droite;
        }
        else if (angle > 45 && angle < 135)
        {
            return PositionRelative.Haut;
        }
        else if (angle > -135 && angle < -45)
        {
            return PositionRelative.Bas;
        }
        else
        {
            return PositionRelative.Gauche;
        }
    }

    private float CalculerAngle(Vector2 pointMillieuStructure1Temp, Vector2 pointMillieuStructure2Temp)
    {
        return Mathf.Atan2(pointMillieuStructure2Temp.y - pointMillieuStructure1Temp.y, pointMillieuStructure2Temp.x - pointMillieuStructure1Temp.x) * Mathf.Rad2Deg;
    }
}