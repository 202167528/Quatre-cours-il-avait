using System;
using System.Text;
using UnityEngine;

public class MapGrille
{
    private int largeur, longueur;
    private Cellule[,] celluleGrille;

    public int Largeur => largeur;

    public int Longueur => longueur;

    public MapGrille(int largeur, int longueur)
    {
        this.largeur = largeur;
        this.longueur = longueur;
        CréerGrille();
    }

    private void CréerGrille()
    {
        celluleGrille = new Cellule[longueur, largeur];
        for (int rangée = 0; rangée < longueur; rangée++)
        for (int colone = 0; colone < largeur; colone++)
            celluleGrille[rangée, colone] = new Cellule(colone, rangée);
    }

    public void DéfinirCellule(int x, int z, TypeObjetCellule typeObjet, bool estPris = false)
    {
        celluleGrille[z, x].TypeObjet = typeObjet;
        celluleGrille[z, x].EstPris = estPris;
    }

    public void DéfinirCellule(float x, float z, TypeObjetCellule typeObjet, bool estPris = false)
        => DéfinirCellule((int)x, (int)z, typeObjet, estPris);


    public bool EstCellulePrise(int x, int z)
        => celluleGrille[z, x].EstPris;


    public bool EstCellulePrise(float x, float z)
        => celluleGrille[(int)z, (int)x].EstPris;


    public int CalculerIndexDepuisCoordinates(int x, int z)
        => x + z * largeur;


    public Vector3 CalculerCoordinatesDepuisIndex(int indexAléatoire)
    {
        int x = indexAléatoire % largeur;
        int z = indexAléatoire / largeur;
        return new Vector3(x, 0, z);
    }

    public bool EstCelluleValide(float x, float z)
        => !(x >= largeur) && !(x < 0) && !(z >= longueur) && !(z < 0);
    //  => x >= largeur || x < 0 || z >= longueur || z < 0 ? false : true;

    public Cellule AvoirCellule(int x, int z)
        => !EstCelluleValide(x, z) ? null : celluleGrille[z, x];

    public Cellule AvoirCellule(float x, float z)
        => AvoirCellule((int)x, (int)z);

    public int CalculerIndexDepuisCoordinates(float x, float z)
        => (int)x + (int)z * largeur;
}