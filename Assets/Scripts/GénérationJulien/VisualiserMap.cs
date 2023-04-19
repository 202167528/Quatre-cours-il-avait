using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisualiserMap : MonoBehaviour
{
    private Transform parent;
    public Color CouleurDébut, CouleurFin;

    public GameObject routeDroite, routeTuileCoin, tuileVide, tuileDébut, tuileFin;
    public GameObject[] environementTuiles;

    Dictionary<Vector3, GameObject> dictionnaireObstacles = new Dictionary<Vector3, GameObject>();

    public bool animer;

    private void Awake() 
        => parent = this.transform;

    public void VisualizeMap(MapGrille grille, MapInformation information)
    {
        for (int i = 0; i < information.chemin.Count; i++)
        {
            var position = information.chemin[i];
            if (position != information.positionFin)
                grille.DéfinirCellule(position.x, position.z, TypeObjetCellule.Route);
        }

        for (int col = 0; col < grille.Largeur; col++)
        {
            for (int rangées = 0; rangées < grille.Longueur; rangées++)
            {
                var cellule = grille.AvoirCellule(col, rangées);
                var position = new Vector3(cellule.X, 0, cellule.Z);

                var index = grille.CalculerIndexDepuisCoordinates(position.x, position.z);
                if (information.obstacleArray[index] && cellule.EstPris == false)
                    cellule.TypeObjet = TypeObjetCellule.Obstacle;


                Direction directionPrécédente = Direction.Aucune;
                Direction prochaineDirection = Direction.Aucune;
                switch (cellule.TypeObjet)
                {
                    case TypeObjetCellule.Vide:
                        CréerIndicateur(position, tuileVide);
                        break;
                    case TypeObjetCellule.Route:
                        if (information.chemin.Count > 0)
                        {
                            directionPrécédente = AvoirDirectionCellulePrécédente(position, information);
                            prochaineDirection = AvoirDirectionCelluleProchaine(position, information);
                        }

                        if (directionPrécédente == Direction.Haut && prochaineDirection == Direction.Droite ||
                            directionPrécédente == Direction.Droite && prochaineDirection == Direction.Haut)
                        {
                            CréerIndicateur(position, routeTuileCoin, Quaternion.Euler(0, 90, 0));
                        }
                        else if (directionPrécédente == Direction.Droite && prochaineDirection == Direction.Bas ||
                                 directionPrécédente == Direction.Bas && prochaineDirection == Direction.Droite)
                        {
                            CréerIndicateur(position, routeTuileCoin, Quaternion.Euler(0, 180, 0));
                        }
                        else if (directionPrécédente == Direction.Bas && prochaineDirection == Direction.Gauche ||
                                 directionPrécédente == Direction.Gauche && prochaineDirection == Direction.Bas)
                        {
                            CréerIndicateur(position, routeTuileCoin, Quaternion.Euler(0, -90, 0));
                        }
                        else if (directionPrécédente == Direction.Gauche && prochaineDirection == Direction.Haut ||
                                 directionPrécédente == Direction.Haut && prochaineDirection == Direction.Gauche)
                        {
                            CréerIndicateur(position, routeTuileCoin);
                        }
                        else if (directionPrécédente == Direction.Droite && prochaineDirection == Direction.Gauche ||
                                 directionPrécédente == Direction.Gauche && prochaineDirection == Direction.Droite)
                        {
                            CréerIndicateur(position, routeDroite, Quaternion.Euler(0, 90, 0));
                        }
                        else
                        {
                            CréerIndicateur(position, routeDroite);
                        }

                        break;
                    case TypeObjetCellule.Obstacle:
                        int indexAléatoire = Random.Range(0, environementTuiles.Length);
                        CréerIndicateur(position, environementTuiles[indexAléatoire]);
                        break;
                    case TypeObjetCellule.Début:
                        if (information.chemin.Count > 0)
                            prochaineDirection = AvoirDirectionDepuisVectors(information.chemin[0], position);


                        if (prochaineDirection == Direction.Droite || prochaineDirection == Direction.Gauche)
                            CréerIndicateur(position, tuileDébut, Quaternion.Euler(0, 90, 0));
                        else
                            CréerIndicateur(position, tuileDébut);

                        break;
                    case TypeObjetCellule.Fin:
                        if (information.chemin.Count > 0)
                        {
                            directionPrécédente = AvoirDirectionCellulePrécédente(position, information);
                            switch (directionPrécédente)
                            {
                                case Direction.Droite:
                                    CréerIndicateur(position, tuileFin, Quaternion.Euler(0, 90, 0));
                                    break;
                                case Direction.Gauche:
                                    CréerIndicateur(position, tuileFin, Quaternion.Euler(0, -90, 0));
                                    break;
                                case Direction.Bas:
                                    CréerIndicateur(position, tuileFin, Quaternion.Euler(0, 180, 0));
                                    break;
                                default:
                                    CréerIndicateur(position, tuileFin);
                                    break;
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
        }
    }

    private Direction AvoirDirectionCelluleProchaine(Vector3 position, MapInformation information)
    {
        int index = information.chemin.FindIndex(a => a == position);
        var prochainePositionCellule = information.chemin[index + 1];
        return AvoirDirectionDepuisVectors(prochainePositionCellule, position);
    }

    private Direction AvoirDirectionCellulePrécédente(Vector3 position, MapInformation information)
    {
        var index = information.chemin.FindIndex(a => a == position);
        var previousCellPosition = Vector3.zero;
        
        if (index > 0)
            previousCellPosition = information.chemin[index - 1];
        else
            previousCellPosition = information.positionDébut;

        return AvoirDirectionDepuisVectors(previousCellPosition, position);
    }

    private Direction AvoirDirectionDepuisVectors(Vector3 positionÀAllerÀ, Vector3 position)
    {
        if (positionÀAllerÀ.x > position.x)
        {
            return Direction.Droite;
        }
        else if (positionÀAllerÀ.x < position.x)
        {
            return Direction.Gauche;
        }
        else if (positionÀAllerÀ.z < position.z)
        {
            return Direction.Bas;
        }

        return Direction.Haut;
    }

    private void CréerIndicateur(Vector3 position, GameObject prefab, Quaternion rotation = new Quaternion())
    {
        var placementPosition = position + new Vector3(.5f, .5f, .5f);
        var element = Instantiate(prefab, placementPosition, rotation);
        element.transform.parent = parent;
        dictionnaireObstacles.Add(position, element);
        if (animer)
        {
            element.AddComponent<DropTween>();
            DropTween.IncreaseDropTime();
        }
    }

    // private void VisualiserAvecPrimitives(MapGrille grille, MapInformation information)
    // {
    //     PlacerPointsDébutEtFin(information);
    //     for (int i = 0; i < information.obstacleArray.Length; i++)
    //         if (information.obstacleArray[i])
    //         {
    //             var positionSurGrille = grille.CalculerCoordinatesDepuisIndex(i);
    //             if (positionSurGrille == information.positionDébut || positionSurGrille == information.positionFin)
    //                 continue;
    //
    //             grille.DéfinirCellule(positionSurGrille.x, positionSurGrille.z, TypeObjetCellule.Obstacle);
    //             if (PlacerObstacleChevalier(information, positionSurGrille))
    //                 continue;
    //
    //             if (dictionnaireObstacles.ContainsKey(positionSurGrille) == false)
    //                 CréerIndicateur(positionSurGrille, Color.white, PrimitiveType.Cube);
    //         }
    // }
    //
    // private bool PlacerObstacleChevalier(MapInformation information, Vector3 positionSurGrille)
    // {
    //     foreach (var chevalier in information.listePiècesChevalier)
    //     {
    //         if (chevalier.Position == positionSurGrille)
    //         {
    //             CréerIndicateur(positionSurGrille, Color.red, PrimitiveType.Cube);
    //             return true;
    //         }
    //     }
    //
    //     return false;
    // }

    // private void PlacerPointsDébutEtFin(MapInformation information)
    // {
    //     CréerIndicateur(information.positionDébut, CouleurDébut, PrimitiveType.Sphere);
    //     CréerIndicateur(information.positionFin, CouleurFin, PrimitiveType.Sphere);
    // }

    // private void CréerIndicateur(Vector3 position, Color color, PrimitiveType sphere)
    // {
    //     var element = GameObject.CreatePrimitive(sphere);
    //     dictionnaireObstacles.Add(position, element);
    //     element.transform.position = position + new Vector3(.5f, .5f, .5f);
    //     element.transform.parent = parent;
    //     var renderer = element.GetComponent<Renderer>();
    //     renderer.material.SetColor("_Color", color);
    //     if (animer)
    //     {
    //         element.AddComponent<DropTween>();
    //         DropTween.IncreaseDropTime();
    //     }
    // }

    public void ClairerMap()
    {
        foreach (var obstacle in dictionnaireObstacles.Values)
            Destroy(obstacle);

        dictionnaireObstacles.Clear();

        if (animer)
            DropTween.ResetTime();
    }
}