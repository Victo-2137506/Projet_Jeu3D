using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Chronometre : MonoBehaviour
{
    public static Chronometre Instance { get; private set; }

    [SerializeField, Tooltip("Affichage du chronomètre")]
    private TextMeshProUGUI affichageChrono;

    [SerializeField, Tooltip("Temps initial du chronomètre (souvent 0)")]
    private float tempsInitial;

    [SerializeField, Tooltip("Temps restant du chronomètre (sera utilisé comme temps écoulé)")]
    private float tempsRestant;

    // Un boolean pour savoir si la voiture a demarrer
    private bool demarreBolide = false;
    // Mettre le chronometre actif ou non
    private bool estActif = false;

    [Header("Actions à la fin du chrono")]
    public UnityEvent evenementFinChrono; // Attache des actions dans l'inspecteur

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // On démarre avec le temps initial
        tempsRestant = tempsInitial;

        // On affiche le temps au lancement
        MettreAJourAffichage();
    }

    private void Update()
    {
        if (!estActif || !demarreBolide)
            return;

        // Diminution du temps
        tempsRestant -= Time.deltaTime;

        // Si moins de 10 secondes, on change la couleur du texte en rouge
        if (tempsRestant <= 10f && affichageChrono.color != Color.red)
        {
            affichageChrono.color = Color.red;
        }

        // Limite à 0
        if (tempsRestant <= 0f)
        {
            tempsRestant = 0f;
            estActif = false;
        }

        // Déclenche l'événement Unity
        evenementFinChrono?.Invoke();

        MettreAJourAffichage();
    }


    /// <summary>
    /// Lorsque le bolide commence à bouger.
    /// </summary>
    public void DemarrerChrono()
    {
        demarreBolide = true;
        estActif = true;
    }

    /// <summary>
    /// Arrête le chronomètre.
    /// </summary>
    public void ArreterChrono()
    {
        estActif = false;
    }

    /// <summary>
    /// Réinitialise le chronomètre.
    /// </summary>
    public void ReinitialiserChrono()
    {
        tempsRestant = tempsInitial;
        estActif = false ;
        demarreBolide = false ;
        if(affichageChrono != null)
        {
            affichageChrono.color = Color.white;
        }
        MettreAJourAffichage();
    }

    /// <summary>
    /// Retourne le temps actuel.
    /// </summary>
    public float GetTemps()
    {
        return tempsRestant;
    }

    /// <summary>
    /// Mise à jour de l'affichage au format mm:ss:cc.
    /// </summary>
    private void MettreAJourAffichage()
    {
        int minutes = Mathf.FloorToInt(tempsRestant / 60f);
        int secondes = Mathf.FloorToInt(tempsRestant % 60f);
        int centiemes = Mathf.FloorToInt((tempsRestant * 100f) % 100f);

        if(affichageChrono != null)
        {
            affichageChrono.text = $"{minutes:00}:{secondes:00}:{centiemes:00}";
        }

    }
}
