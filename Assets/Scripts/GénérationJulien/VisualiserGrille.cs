using UnityEngine;

public class VisualiserGrille : MonoBehaviour
{
    public GameObject prefabsSol;

    public void VisualiséGrille(int largeur, int longueur)
    {
        Vector3 position = new Vector3(largeur / 2f, 0, longueur / 2f);
        Quaternion rotation = Quaternion.Euler(90, 0, 0);
        var côté = Instantiate(prefabsSol, position, rotation);
        côté.transform.localScale = new Vector3(largeur, longueur, 1);
    }
}