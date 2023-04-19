using SVS.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandidatMap
{
    private MapGrille grille;
    private int nbPièces = 0;
    private bool[] obstaclesArray = null;
    private Vector3 pointDébut, pointFin;
    private List<PiècesChevalier> listePiècesChevalier;
    private List<Vector3> chemin = new List<Vector3>();


    public MapGrille Grille => grille;
    
    public bool[] ObstaclesArray => obstaclesArray;

    public CandidatMap(MapGrille grille, int nbPièces)
    {
        this.nbPièces = nbPièces;
        this.grille = grille;
    }

    public void CréerMap(Vector3 positionDébut, Vector3 positionFin, bool réparationAutomatique = false)
    {
        this.pointDébut = positionDébut;
        this.pointFin = positionFin;
        obstaclesArray = new bool[grille.Largeur * grille.Longueur];
        this.listePiècesChevalier = new List<PiècesChevalier>();
        PlacerAléatoirementChevalier(this.nbPièces);

        PlacerObstacles();
        TrouverChemin();

        if (réparationAutomatique)
            Réparer();
    }

    private void TrouverChemin() =>
        this.chemin = Astar.AvoirChemin(pointDébut, pointFin, obstaclesArray, grille);


    private bool RegarderSiPositionPeutÊtreObstacle(Vector3 position)
    {
        if (position == pointDébut || position == pointFin)
            return false;

        int index = grille.CalculerIndexDepuisCoordinates(position.x, position.z);

        return obstaclesArray[index] == false;
    }

    private void PlacerAléatoirementChevalier(int nPièces)
    {
        var conter = nbPièces;
        var essayerLimitPlacementChevalier = 100;
        while (conter > 0 && essayerLimitPlacementChevalier > 0)
        {
            var indexAléatoire = Random.Range(0, obstaclesArray.Length);
            if (obstaclesArray[indexAléatoire] == false)
            {
                var coordonner = grille.CalculerCoordinatesDepuisIndex(indexAléatoire);
                if (coordonner == pointDébut || coordonner == pointFin)
                    continue;

                obstaclesArray[indexAléatoire] = true;
                listePiècesChevalier.Add(new PiècesChevalier(coordonner));
                conter--;
            }

            essayerLimitPlacementChevalier--;
        }
    }

    private void PlacerObstaclesPourCeChevalier(PiècesChevalier chevalier)
    {
        foreach (var position in PiècesChevalier.listeMouvementsPossible)
        {
            var nouvellePosition = chevalier.Position + position;
            if (grille.EstCelluleValide(nouvellePosition.x, nouvellePosition.z) && RegarderSiPositionPeutÊtreObstacle(nouvellePosition))
                obstaclesArray[grille.CalculerIndexDepuisCoordinates(nouvellePosition.x, nouvellePosition.z)] = true;
            
        }
    }

    private void PlacerObstacles()
    {
        foreach (var knight in listePiècesChevalier)
            PlacerObstaclesPourCeChevalier(knight);
    }

    public MapInformation RetournerMapInformation()
    {
        return new MapInformation
        {
            obstacleArray = this.obstaclesArray,
            listePiècesChevalier = listePiècesChevalier,
            positionDébut = pointDébut,
            positionFin = pointFin,
            chemin = this.chemin
        };
    }

    public List<Vector3> Réparer()
    {
        int nbObstacle = obstaclesArray.Where(obstacle => obstacle).Count();
        List<Vector3> listeObstaclesÀBouger = new List<Vector3>();
        if (chemin.Count <= 0)
        {
            do
            {
                int indexObstacleÀBouger = Random.Range(0, nbObstacle);
                for (int i = 0; i < obstaclesArray.Length; i++)
                {
                    if (obstaclesArray[i])
                    {
                        if (indexObstacleÀBouger == 0)
                        {
                            obstaclesArray[i] = false;
                            listeObstaclesÀBouger.Add(grille.CalculerCoordinatesDepuisIndex(i));
                            break;
                        }

                        indexObstacleÀBouger--;
                    }
                }

                TrouverChemin();
            } while (this.chemin.Count <= 0);
        }

        foreach (var PositionObstacle in listeObstaclesÀBouger)
        {
            if (chemin.Contains(PositionObstacle) == false)
            {
                int index = grille.CalculerIndexDepuisCoordinates(PositionObstacle.x, PositionObstacle.z);
                obstaclesArray[index] = true;
            }
        }

        return listeObstaclesÀBouger;
    }
}