using UnityEngine;

/// <summary>
/// ScriptableObject pour définir les configurations du chronomètre.
/// Permet de créer différentes configurations de course.
/// </summary>
[CreateAssetMenu(fileName = "ConfigChronometre", menuName = "Course/Configuration Chronometre")]
public class ConfigChrono : ScriptableObject
{
    [SerializeField, Tooltip("Temps initial du chronomètre en secondes")]
    private float tempsInitial = 120f;
    public float TempsInitial => tempsInitial;

    [SerializeField, Tooltip("Temps perdu en secondes quand on touche une barrière")]
    private float penaliteBarriere = 2f;
    public float PenaliteBarriere => penaliteBarriere;

    [SerializeField, Tooltip("Seuil de temps restant pour changer la couleur en rouge")]
    private float derniereSeconde = 10f;
    public float DerniereSeconde => derniereSeconde;

    [SerializeField, Tooltip("Couleur normale du chronomètre")]
    private Color couleurNormale = Color.white;
    public Color CouleurNormale => couleurNormale;

    [SerializeField, Tooltip("Couleur de fin du chronomètre")]
    private Color couleurAlerte = Color.red;
    public Color CouleurAlerte => couleurAlerte;

}