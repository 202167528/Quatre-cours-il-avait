using System;
using System.Collections.Generic;
using UnityEngine;

namespace SVS.AI
{
    public class VertexPosition : IEquatable<VertexPosition>, IComparable<VertexPosition>
    {
        public static List<Vector2Int> voisinPossible = new List<Vector2Int>
        {
            new Vector2Int(0,-1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(-1,0)
        };

        public float coûtTotale, coûtEstimer;
        public VertexPosition vertexPrécédent = null;
        private Vector3 position;
        private bool estPris;

        public int X  => (int)Position.x; 
        public int Z => (int)Position.z; 
        public Vector3 Position  => position; 
        public bool EstPris => estPris;

        public VertexPosition(Vector3 position, bool isTaken = false)
        {
            this.position = position;
            this.estPris = isTaken;
            this.coûtEstimer = 0;
            this.coûtTotale = 1;
        }

        public int GetHashCOde(VertexPosition obj)
            => obj.GetHashCode();
		
        public override int GetHashCode()
            => position.GetHashCode();
		

        public int CompareTo(VertexPosition other)
        {
            if (this.coûtEstimer < other.coûtEstimer) return -1;
            if (this.coûtEstimer > other.coûtEstimer) return 1;
            return 0;
        }

        public bool Equals(VertexPosition other)
            => Position == other.Position;
		
    }
}