using UnityEngine;

public class Cellule
{
    private int x, z;
    private bool estPris;
    private TypeObjetCellule typeObjet;

    public int X => x;

    public int Z => z;

    public bool EstPris
    {
        get => estPris;
        set => estPris = value;
    }

    public TypeObjetCellule TypeObjet
    {
        get => typeObjet;
        set => typeObjet = value;
    }

    public Cellule(int x, int z)
    {
        this.x = x;
        this.z = z;
        this.typeObjet = TypeObjetCellule.Vide;
        estPris = false;
    }
}

public enum TypeObjetCellule
{
    Vide,
    Route,
    Obstacle,
    Début,
    Fin
}