using UnityEngine;

/// <summary>
/// Script pour les zones qui détecte le passage du bolide et incrémente le compteur de tours.
/// </summary>
public class ZoneLigne : MonoBehaviour
{
    [field: SerializeField]
    public Tour Tour { get; private set; }

    /// <summary>
    /// Lorsque le bolide traverse la zone, on incrémente les tours.
    /// </summary>
    /// <param name="other">L'objet qui entre dans la zone</param>
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet possède un script Bolide
        if (other.TryGetComponent(out Bolide bolide))
        {
            Tour.IncrementerTour();
        }
    }
}
