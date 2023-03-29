using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    // On assigne tous les renderers dans l'inspecteur
    [SerializeField] private List<Renderer> renderers;

    [SerializeField] private Color color = Color.white;

    // Liste de helper pour mettre en mémoire tous les matériaux ofd de cet objet
    private List<Material> materials;
    
    // Reçois tous les matériaux pour chaque renderer
    private void Awake()
    {
        materials = new List<Material>();
        foreach (var rend in renderers)
        {
            materials.AddRange(new List<Material>(rend.materials));
        }
    }

    public void ToggleHighlight(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                // On doit activer EMISSION
                material.EnableKeyword("_EMISSION");
                // avant qu'on set chaque couleur
                material.SetColor("_EmissionColor", color);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                // On peut juste désactiver EMISSION
                // Si on n'utilise pas la couleur de l'emission
                material.DisableKeyword("_EMISSION");
            }
        }
        
    }
}

