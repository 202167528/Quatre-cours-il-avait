using System.Collections.Generic;
using UnityEngine;

public struct MapInformation
{
    public bool[] obstacleArray;
    public List<PiècesChevalier> listePiècesChevalier;
    public Vector3 positionDébut;
    public Vector3 positionFin;
    public List<Vector3> chemin;
}