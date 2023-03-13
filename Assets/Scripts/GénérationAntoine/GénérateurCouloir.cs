using System;
using System.Collections.Generic;
using System.Linq;

public class GénérateurCouloir
{
    public List<Noeuds> CréerCouloir(List<NoeudPièce> touteEspaceNoeuds, int largeurCouloir)
    {
        List<Noeuds> listeCouloir = new List<Noeuds>();
        Queue<NoeudPièce> structureÀVérifier = new Queue<NoeudPièce>(touteEspaceNoeuds.OrderByDescending(noeud => noeud.IndexArbre).ToList());
        while (structureÀVérifier.Count > 0)
        {
            var noeud = structureÀVérifier.Dequeue();
            if (noeud.ChildenListNoeuds.Count == 0)
            {
                continue;
            }
            NoeudCouloir couloir = new NoeudCouloir(noeud.ChildenListNoeuds[0], noeud.ChildenListNoeuds[1], largeurCouloir);
            listeCouloir.Add(couloir);
        }
        return listeCouloir;

    }
}