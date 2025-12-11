using UnityEngine;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Script qui gère le nombre de tours actuels et met à jour l'affichage.
/// Suggestion de Claude IA 
/// </summary>
public class Tour : MonoBehaviour
{
    [field: SerializeField, Tooltip("Nombre total de tours pour la course.")]
    public int ToursTotal { get; private set; }

    [field: SerializeField, Tooltip("Texte UI affichant les tours.")]
    public TMP_Text TexteTours { get; private set; }

    public int ToursActuels { get; private set; }

    [SerializeField, Tooltip("Gestion des écrans pour la fin de partie.")]
    private GestionFinDePartie gestion;

    [Header("Événements")]
    [Tooltip("Déclenché chaque fois qu'un tour est complété")]
    public UnityEvent evenementFinDeTour;

    private void Start()
    {
        MettreAJourTexte();
    }

    /// <summary>
    /// Incrémente le tour
    /// </summary>
    public void IncrementerTour()
    {
        ToursActuels++;
        MettreAJourTexte();

        // Déclenche l'événement
        evenementFinDeTour?.Invoke();

        // Quand il atteint le dernier tour (ex: 3/3)
        if (ToursActuels == ToursTotal)
        {
            MessageTour.Instance.AfficherMessage();
        }

        // Victoire
        if (ToursActuels > ToursTotal)
        {
            ArreterTemps();
        }
    }

    private void MettreAJourTexte()
    {
        if (TexteTours != null)
            TexteTours.text = $"{ToursActuels} / {ToursTotal}";
    }

    private void ArreterTemps()
    {
        Debug.Log("Temps arrêté !");
        Time.timeScale = 0f;
        if (gestion != null)
            gestion.AfficherVictoire();
    }
}