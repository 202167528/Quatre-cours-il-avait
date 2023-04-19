using System;
using System.Collections.Generic;
using UnityEngine;

namespace SVS.AI
{
    public static class Astar
    {
        public static List<Vector3> AvoirChemin(Vector3 début, Vector3 fin, bool[] obstaclesArray, MapGrille grille)
        {
            VertexPosition vertexDébut = new VertexPosition(début);
            VertexPosition vertexFin = new VertexPosition(fin);

            List<Vector3> chemin = new List<Vector3>();

            List<VertexPosition> listeOuverte = new List<VertexPosition>();
            HashSet<VertexPosition> listeFermé = new HashSet<VertexPosition>();

            vertexDébut.coûtEstimer = ManhattanDistance(vertexDébut, vertexFin);

            listeOuverte.Add(vertexDébut);

            VertexPosition vertexPrésentement = null;
            while (listeOuverte.Count > 0)
            {
                listeOuverte.Sort();
                vertexPrésentement = listeOuverte[0];
                if (vertexPrésentement.Equals(vertexFin))
                {
                    while (vertexPrésentement != vertexDébut)
                    {
                        chemin.Add(vertexPrésentement.Position);
                        vertexPrésentement = vertexPrésentement.vertexPrécédent;
                    }

                    chemin.Reverse();
                    break;
                }

                var arrayDeVoisin = TrouverVoisinPour(vertexPrésentement, grille, obstaclesArray);
                foreach (var voisin in arrayDeVoisin)
                {
                    if (voisin == null || listeFermé.Contains(voisin))
                        continue;
                    if (voisin.EstPris == false)
                    {
                        var coûtTotale = vertexPrésentement.coûtTotale + 1;
                        var coûtEstimerVoisin = ManhattanDistance(voisin, vertexFin);
                        voisin.coûtTotale = coûtTotale;
                        voisin.vertexPrécédent = vertexPrésentement;
                        voisin.coûtEstimer = coûtTotale + coûtEstimerVoisin;
                        if (listeOuverte.Contains(voisin) == false)
                        {
                            listeOuverte.Add(voisin);
                        }
                    }
                }

                listeFermé.Add(vertexPrésentement);
                listeOuverte.Remove(vertexPrésentement);
            }

            return chemin;
        }

        private static VertexPosition[] TrouverVoisinPour(VertexPosition vertexPrésentement, MapGrille grille,
            bool[] obstaclesArray)
        {
            VertexPosition[] arrayDeVoisin = new VertexPosition[4];

            int arrayIndex = 0;
            foreach (var voisinPossible in VertexPosition.voisinPossible)
            {
                Vector3 position = new Vector3(vertexPrésentement.X + voisinPossible.x, 0,
                    vertexPrésentement.Z + voisinPossible.y);
                if (grille.EstCelluleValide(position.x, position.z))
                {
                    int index = grille.CalculerIndexDepuisCoordinates(position.x, position.z);
                    arrayDeVoisin[arrayIndex] = new VertexPosition(position, obstaclesArray[index]);
                    arrayIndex++;
                }
            }

            return arrayDeVoisin;
        }

        private static float ManhattanDistance(VertexPosition vertexDébut, VertexPosition vertexFin)
        {
            return Mathf.Abs(vertexDébut.X - vertexFin.X) + Mathf.Abs(vertexDébut.Z - vertexFin.Z);
        }
    }
}