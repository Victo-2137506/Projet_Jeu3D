using UnityEngine;

/// <summary>
/// ScriptableObject pour définir les configurations de ralentissement.
/// Inspiré des notes de cours : https://cours-alexandre-ouellet.github.io/jeux-3d/programmer/scriptable/
/// </summary>
[CreateAssetMenu(fileName = "Ralentissement", menuName = "Course/Ralentissement")]
public class Ralentissement : ScriptableObject
{
    [SerializeField, Tooltip("Multiplicateur de vitesse (< 1 pour ralentir)")]
    private float multiplicateur;
    public float Multiplicateur => multiplicateur;

    [SerializeField, Tooltip("Durée de l'effet en secondes")]
    private float duree;
    public float Duree => duree;
}