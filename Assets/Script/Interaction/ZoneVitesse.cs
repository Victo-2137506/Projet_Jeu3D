
using UnityEngine;

/// <summary>
/// Script pour les zones de ralentissement.
/// </summary>
public class ZoneVitesse : MonoBehaviour
{
    // Appele la configuration de ralentissement
    [SerializeField, Tooltip("Configuration du ralentissement à appliquer")]
    private Ralentissement configuration;

    /// <summary>
    /// Quand le bolide passe dessus, il applique le ralentissement
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (configuration == null)
        {
            Debug.LogWarning("Aucune configuration de ralentissement assignée");
            return;
        }

        Bolide bolide = other.GetComponent<Bolide>();
        if (bolide != null)
        {
            bolide.StartCoroutine(bolide.AppliquerRalentissement(
                configuration.Multiplicateur,
                configuration.Duree
            ));
        }
    }
}