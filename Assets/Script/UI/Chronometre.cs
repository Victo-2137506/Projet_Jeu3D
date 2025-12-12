using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script pour le chronomètre
/// Code inspiré du deuxième examen, livreur de pizza
/// </summary>
public class Chronometre : MonoBehaviour
{
    // Singleton Chronometre
    public static Chronometre Instance { get; private set; }

    // Appele le ScriptableObject du chronomètre
    [SerializeField, Tooltip("Configuration du chronomètre")]
    private ConfigChrono configuration;

    [SerializeField, Tooltip("Affichage du chronomètre")]
    private TextMeshProUGUI affichageChrono;

    [SerializeField, Tooltip("Gestion des écrans pour la fin de partie.")]
    private GestionFinDePartie gestion;

    [SerializeField, Tooltip("Vitesse du clignotement")]
    private float vitesseClignotement = 2f;

    [SerializeField, Tooltip("Temps restant du chronomètre")]
    private float tempsRestant;

    private bool demarreBolide = false;
    private bool estActif = false;

    private Coroutine routineClignotement;

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
        MettreAJourAffichage();
    }

    /// <summary>
    /// Initialise le chronomètre avec la configuration de difficulté active.
    /// Appelé par le DifficultyManager après la validation du menu.
    /// </summary>
    public void InitialiserChrono(ConfigChrono nouvelleConfig)
    {
        configuration = nouvelleConfig;

        if (configuration == null)
        {
            Debug.LogError("Impossible d'initialiser le chronomètre : Configuration manquante.");
            return;
        }

        tempsRestant = configuration.TempsInitial;

        if (affichageChrono != null)
        {
            // Couleur normale codée en dur
            affichageChrono.color = Color.white;
        }

        MettreAJourAffichage();
    }

    private void Update()
    {
        if (!estActif || !demarreBolide || configuration == null)
            return;

        // Diminution du temps
        tempsRestant -= Time.deltaTime;

        // Si moins de 10 secondes on démarre le clignotement
        if (tempsRestant <= 10f && routineClignotement == null)
        {
            routineClignotement = StartCoroutine(ClignoterTexte());
        }

        // Limite à 0
        if (tempsRestant <= 0f)
        {
            tempsRestant = 0f;
            estActif = false;

            if (routineClignotement != null)
            {
                StopCoroutine(routineClignotement);
                routineClignotement = null;
                // Définir la couleur finale sur rouge
                affichageChrono.color = Color.red;
            }

            if (gestion != null)
                gestion.AfficherDefaite();

            Time.timeScale = 0f;
        }

        MettreAJourAffichage();
    }

    /// <summary>
    /// Permet de faire le clignotement du chronomètre quand il reste 10 secondes.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ClignoterTexte()
    {
        if (affichageChrono == null)
            yield break;

        float temps = 0f;

        while (true)
        {
            temps += Time.deltaTime * vitesseClignotement;
            float clignotement = Mathf.PingPong(temps, 1f); // Mathf.PingPong est suggérer par Claude IA
            affichageChrono.color = Color.Lerp(Color.white, Color.red, clignotement);
            yield return null;
        }
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

        if (routineClignotement != null)
        {
            StopCoroutine(routineClignotement);
            routineClignotement = null;
        }
    }

    /// <summary>
    /// Retourne le temps actuel.
    /// </summary>
    public float GetTemps()
    {
        return tempsRestant;
    }

    /// <summary>
    /// Retire du temps quand il touche une barriere.
    /// </summary>
    /// <param name="penalite"></param>
    public void RetirerTemps(float penalite)
    {
        tempsRestant -= penalite;

        if (tempsRestant < 0f)
            tempsRestant = 0f;

        MettreAJourAffichage();
    }

    /// <summary>
    /// Mise à jour de l'affichage au format mm:ss:cc.
    /// </summary>
    private void MettreAJourAffichage()
    {
        int minutes = Mathf.FloorToInt(tempsRestant / 60f);
        int secondes = Mathf.FloorToInt(tempsRestant % 60f);
        int centiemes = Mathf.FloorToInt((tempsRestant * 100f) % 100f);

        if (affichageChrono != null)
        {
            affichageChrono.text = $"{minutes:00}:{secondes:00}:{centiemes:00}";
        }
    }
}