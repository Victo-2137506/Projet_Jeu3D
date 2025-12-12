using UnityEngine;

/// <summary>
/// ScriptableObject pour définir les configurations du chronomètre.
/// Permet de créer différentes configurations de course.
/// Inspiré des notes de cours : https://cours-alexandre-ouellet.github.io/jeux-3d/programmer/scriptable/
/// </summary>
[CreateAssetMenu(fileName = "ConfigChronometre", menuName = "Course/Configuration Chronometre")]
public class ConfigChrono : ScriptableObject
{
    [SerializeField, Tooltip("Temps initial du chronomètre en secondes")]
    private float tempsInitial;
    public float TempsInitial => tempsInitial;

    [SerializeField, Tooltip("Temps perdu en secondes quand on touche une barrière")]
    private float penaliteBarriere;
    public float PenaliteBarriere => penaliteBarriere;
}