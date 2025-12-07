using UnityEngine;
using TMPro;
using Unity.VisualScripting.FullSerializer;

/// <summary>
/// Gère le nombre de tours actuels et met à jour l'affichage.
/// </summary>
public class Tour : MonoBehaviour
{
    [field: SerializeField, Tooltip("Nombre total de tours pour la course.")]
    public int ToursTotal { get; private set; }

    [field: SerializeField, Tooltip("Texte UI affichant les tours.")]
    public TMP_Text TexteTours { get; private set; }

    public int ToursActuels { get; private set; }

    private void Start()
    {
        MettreAJourTexte();
    }

    public void IncrementerTour()
    {
        ToursActuels++;
        MettreAJourTexte();

        // Quand il atteint le dernier tour (ex: 3/3)
        if (ToursActuels == ToursTotal)
        {
            MessageTour.Instance.AfficherMessage();
        }

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
    }
}
