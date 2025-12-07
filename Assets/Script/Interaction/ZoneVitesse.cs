using UnityEngine;

public class ZoneBoost : MonoBehaviour
{
    [Header("Paramètres de la zone")]
    public float multiplicateur = 2f;   // >1 = boost, <1 = ralentissement
    public float duree = 2f;

    private void OnTriggerEnter(Collider other)
    {
        Bolide bolide = other.GetComponent<Bolide>();
        if (bolide != null)
        {
            bolide.StartCoroutine(bolide.AppliquerRalentissementProgressif(multiplicateur, duree));
        }
    }
}
