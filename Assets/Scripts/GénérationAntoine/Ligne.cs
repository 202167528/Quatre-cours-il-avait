using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ligne
{
    Orientation orientation;
    Vector2Int coordonées;
    public Ligne(Orientation orientation, Vector2Int coordonées)
    {
        this.orientation = orientation;
        this.coordonées = coordonées;
    }

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coordonées { get => coordonées; set => coordonées = value; }
}
public enum Orientation
{
    Horizontale = 0,
    Verticale = 1
}