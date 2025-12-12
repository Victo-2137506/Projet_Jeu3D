using UnityEngine;

/// <summary>
/// Script pour les zones de ralentissement.
/// </summary>
public class ZoneVitesse : MonoBehaviour
{
    // Appele la configuration de ralentissement
    [SerializeField]
    private Ralentissement configuration;

/// <summary>
/// Quand le bolide passe dessus, il applique le ralentissement
/// </summary>
/// <param name="other"></param>
private void OnTriggerEnter(Collider other)
{
    Ralentissement configRalentissement = MenuDifficulte.Instance.Ralentissement;
    
    // Vérification de la présence du composant Bolide
    Bolide bolide = other.GetComponent<Bolide>();
    if (bolide != null)
    {
        // L'appel à la coroutine du Bolide
        bolide.StartCoroutine(bolide.AppliquerRalentissement(
            configRalentissement.Multiplicateur,
            configRalentissement.Duree
        ));
    }
}
}