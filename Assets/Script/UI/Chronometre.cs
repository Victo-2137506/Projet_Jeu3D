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
        if (configuration == null)
        {
            Debug.LogError("Aucune configuration de chronomètre assignée !");
            return;
        }

        // On démarre avec le temps initial
        tempsRestant = configuration.TempsInitial;

        if (affichageChrono != null)
        {
            affichageChrono.color = configuration.CouleurNormale;
        }

        // On affiche le temps au lancement
        MettreAJourAffichage();
    }

    private void Update()
    {
        if (!estActif || !demarreBolide || configuration == null)
            return;

        // Diminution du temps
        tempsRestant -= Time.deltaTime;

        // Si moins du seuil d'alerte, on démarre le clignotement
        if (tempsRestant <= configuration.DerniereSeconde && routineClignotement == null)
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
            }

            if (gestion != null)
                gestion.AfficherDefaite();

            Time.timeScale = 0f;
        }

        MettreAJourAffichage();
    }

    /// <summary>
    /// Permet de faire le clignotement du chrnomètre quand il arrive à 10 secondes.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ClignoterTexte()
    {
        if (affichageChrono == null || configuration == null)
            yield break;

        float temps = 0f;

        while (true)
        {
            temps += Time.deltaTime * vitesseClignotement;
            float clignotement = Mathf.PingPong(temps, 1f); // Mathf.PingPong est suggérer par Claude IA
            affichageChrono.color = Color.Lerp(configuration.CouleurNormale, configuration.CouleurAlerte, clignotement);
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