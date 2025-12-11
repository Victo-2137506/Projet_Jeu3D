using UnityEngine;

/// <summary>
/// ScriptableObject pour définir les configurations de ralentissement.
/// </summary>
[CreateAssetMenu(fileName = "Ralentissement", menuName = "Course/Ralentissement")]
public class Ralentissement : ScriptableObject
{
    [SerializeField, Tooltip("Multiplicateur de vitesse (< 1 pour ralentir)")]
    private float multiplicateur = 0.5f;
    public float Multiplicateur => multiplicateur;

    [SerializeField, Tooltip("Durée de l'effet en secondes")]
    private float duree = 2f;
    public float Duree => duree;
}