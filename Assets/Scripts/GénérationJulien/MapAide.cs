using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MapAide
{
    public static void ChoisirEtDéfinirAléatoirementPositionDébutEtFin(MapGrille grille, ref Vector3 positionDébut,
        ref Vector3 positionFin)
    {
        positionDébut = ChoisirAléatoirementPositionSurCôtéGrille(grille, positionDébut, Direction.Bas);
        positionFin = ChoisirAléatoirementPositionSurCôtéGrille(grille, positionDébut, Direction.Haut);

        grille.DéfinirCellule(positionDébut.x, positionDébut.z, TypeObjetCellule.Début);
        grille.DéfinirCellule(positionFin.x, positionFin.z, TypeObjetCellule.Fin);
    }

    private static Vector3 ChoisirAléatoirementPositionSurCôtéGrille(MapGrille grille, Vector3 positionDébut,
        Direction direction)
    {
        Vector3 position = Vector3.zero;
        switch (direction)
        {
            case Direction.Haut:
                position = new Vector3(Random.Range(grille.Largeur * 3 / 4, grille.Largeur - 3), 0, grille.Longueur - 1);
                break;

            case Direction.Bas:
                position = new Vector3(Random.Range(3, grille.Largeur / 4), 0, 0);
                break;

            default:
                break;
        }

        return position;
    }
}