using UnityEngine;
public class NoeudPièce : Noeuds
{
    public NoeudPièce(Vector2Int coinBasGauche, Vector2Int coinHautDroit, Noeuds noeudParent, int index) : base(noeudParent)
    {
        this.CoinBasGauche = coinBasGauche;
        this.CoinHautDroit = coinHautDroit;
        this.CoinBasDroit = new Vector2Int(coinHautDroit.x, coinBasGauche.y);
        this.CoinHautGauche = new Vector2Int(coinBasGauche.x, coinHautDroit.y);
        this.IndexArbre = index;
    }
    public int Largeur { get => (int)(CoinHautDroit.x - CoinBasGauche.x); }
    public int Hauteur { get => (int)(CoinHautDroit.y - CoinBasGauche.y); }
}