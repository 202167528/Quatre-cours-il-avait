using UnityEngine;

public class MapGenerateur : MonoBehaviour
{
    public VisualiserGrille visualisateurGrille;
    public VisualiserMap visualiserMap;

    private Direction côtéDébut = Direction.Bas, côtéFin = Direction.Haut;
    [Range(1, 100)] public int nbPièces;

    public bool réparationAutomatique = true;

    private Vector3 positionDébut, positionFin;
    private CandidatMap map;

    [Range(3, 200)] public int largeur, longueur = 100;
    private MapGrille grille;

    private void Start()
    {
        visualisateurGrille.VisualiséGrille(largeur, longueur);
        GénérerNouvelleMap();
    }

    public void GénérerNouvelleMap()
    {
        visualiserMap.ClairerMap();

        grille = new MapGrille(largeur, longueur);

        MapAide.ChoisirEtDéfinirAléatoirementPositionDébutEtFin(grille, ref positionDébut, ref positionFin);

        map = new CandidatMap(grille, nbPièces);
        map.CréerMap(positionDébut, positionFin, réparationAutomatique);
        visualiserMap.VisualizeMap(grille, map.RetournerMapInformation());
    }

    public void EssayerRéparer()
    {
        if (map != null)
        {
            var listeObstaclesÀBouger = map.Réparer();
            if (listeObstaclesÀBouger.Count > 0)
            {
                visualiserMap.ClairerMap();
                visualiserMap.VisualizeMap(grille, map.RetournerMapInformation());
            }
        }
    }
}