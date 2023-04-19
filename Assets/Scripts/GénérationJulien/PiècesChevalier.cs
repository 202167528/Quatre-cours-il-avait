using System.Collections.Generic;
using UnityEngine;

public class PiècesChevalier
{
    public static List<Vector3> listeMouvementsPossible = new List<Vector3>
    {
        new Vector3(-1, 0, 2),
        new Vector3(1, 0, 2),
        new Vector3(-1, 0, -2),
        new Vector3(1, 0, -2),
        new Vector3(-2, 0, -1),
        new Vector3(-2, 0, 1),
        new Vector3(2, 0, -1),
        new Vector3(2, 0, 1)
    };

    private Vector3 position;

    public Vector3 Position
    {
        get => position;
        set => position = value;
    }

    public PiècesChevalier(Vector3 position)
    {
        this.Position = position;
    }
}