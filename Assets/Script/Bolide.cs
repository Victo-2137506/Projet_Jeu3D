using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Script de contrôle pour le bolide.
/// Code inspiré du deuxième examen, livreur de pizza.
/// </summary>
public class Bolide : MonoBehaviour
{
    #region Paramètres du véhicule
    [Header("Paramètres de déplacement")]
    [field: SerializeField, Tooltip("Limite de vitesse min/max du bolide (négative = reculons)")]
    public Vector2 LimiteVitesse { get; private set; }

    [field: SerializeField, Tooltip("Accélération en avant")]
    public float Acceleration { get; private set; }

    [field: SerializeField, Tooltip("Accélération en reculons")]
    public float AccelerationReculons { get; private set; }

    [field: SerializeField, Tooltip("Friction appliquée lorsque non accélérant")]
    public float AccelerationFriction { get; private set; }

    [field: SerializeField, Tooltip("Force de freinage lorsqu'on appuie en arrière")]
    public float Freinage { get; private set; }

    [field: SerializeField, Tooltip("Vitesse de rotation du véhicule")]
    public float VitesseVirage { get; private set; }

    [field: SerializeField, Tooltip("Facteur de virage selon la vitesse (x=min, y=max)")]
    public Vector2 FacteurTauxVirage { get; private set; }

    [SerializeField, Tooltip("Temps perdu en secondes quand on touche une barrière")]
    private float penaliteTemps;
    #endregion

    public UnityEvent evenementCollisionBarriere;

    private Vector2 controles;
    private float vitesseActuelle;

    // Généré par Claude IA. (2025). https://claude.ai/

    private float dernierTempsCollision = -999f;
    private float delaiCollision = 3f;

    // Fin du code généré

    private void Awake()
    {
        controles = Vector2.zero;
        vitesseActuelle = 0f;
    }

    private void Update()
    {
        Deplacement();
        Rotation();
    }

    #region Gestion input
    /// <summary>
    /// Recoit les entrées du joueur
    /// </summary>
    public void OnDeplacement(InputAction.CallbackContext contexte)
    {
        controles = contexte.ReadValue<Vector2>();
    }
    #endregion

    #region Déplacement
    private void Deplacement()
    {
        float inputAvantArriere = controles.y;

        // --- ACCÉLÉRATION ---
        if (inputAvantArriere > 0f)
        {
            vitesseActuelle += Acceleration * Time.deltaTime;
        }
        // --- FREINAGE ET RECULONS ---
        else if (inputAvantArriere < 0f)
        {
            if (vitesseActuelle > 0f)
            {
                vitesseActuelle -= Freinage * Time.deltaTime;
            }
            else
            {
                vitesseActuelle -= AccelerationReculons * Time.deltaTime;
            }
        }
        // --- FRICTION ---
        else
        {
            if (vitesseActuelle > 0f)
            {
                vitesseActuelle -= AccelerationFriction * Time.deltaTime;
            }
            else if (vitesseActuelle < 0f)
            {
                vitesseActuelle += AccelerationFriction * Time.deltaTime;
            }
        }

        // Clamp vitesse
        vitesseActuelle = Mathf.Clamp(vitesseActuelle, LimiteVitesse.x, LimiteVitesse.y);

        // Si il a une petite vitesse il s'arrête
        if (Mathf.Abs(vitesseActuelle) < 0.01f)
            vitesseActuelle = 0f;

        // Déplacement
        transform.position += transform.forward * vitesseActuelle * Time.deltaTime;

        // Si le chronomètre n'existe pas
        if (!Chronometre.Instance) return;

        // Dès que le bolide bouge lance le chronomètre
        if (vitesseActuelle != 0f)
        {
            Chronometre.Instance.DemarrerChrono();
        }
    }
    #endregion

    #region Rotation
    private void Rotation()
    {
        if (Mathf.Abs(vitesseActuelle) < 0.05f)
            return;

        float inputRotation = controles.x;

        // Facteur de virage selon la vitesse
        float t = Mathf.InverseLerp(0f, LimiteVitesse.y, Mathf.Abs(vitesseActuelle));
        float facteurVirage = Mathf.Lerp(FacteurTauxVirage.x, FacteurTauxVirage.y, t);

        transform.Rotate(
            Vector3.up,
            inputRotation * VitesseVirage * facteurVirage * Time.deltaTime
        );
    }
    #endregion

    #region Collisions
    // Gère les collision contre les barrières pour avoir une pénalité de temps
    // Suggestion d'un ami pour un temps de récupération après une pénalité puis code inspiré de Claude AI
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Barriere"))
        {
            // Vérifie si assez de temps s'est écoulé depuis la dernière collision
            if (Time.time - dernierTempsCollision >= delaiCollision)
            {
                // On récupère la configuration du chronomètre
                ConfigChrono config = MenuDifficulte.Instance.ConfigChrono;

                if (config != null && Chronometre.Instance != null)
                {
                    // Utilise la pénalité lue de la configuration
                    Chronometre.Instance.RetirerTemps(config.PenaliteBarriere);
                }

                // Arrête le bolide
                vitesseActuelle = 0f;

                evenementCollisionBarriere?.Invoke();

                // Enregistre le moment de cette collision
                dernierTempsCollision = Time.time;
            }
        }
    }
    #endregion
    /// <summary>
    /// Applique l'effet de ralentissement
    /// </summary>
    /// <param name="multiplicateur">Multiplicateur de ralentissement (0.5f)</param>
    /// <param name="duree">Durée du ralentissement</param>
    /// <returns></returns>
    public IEnumerator AppliquerRalentissement(float multiplicateur, float duree)
    {
        float vitesseOriginale = vitesseActuelle;

        // Applique le multiplicateur
        vitesseActuelle *= multiplicateur;

        yield return new WaitForSeconds(duree);

        // Retour à la vitesse normale (le default est que la vitesse normale revient d'un coup)
        vitesseActuelle = Mathf.Clamp(vitesseOriginale, LimiteVitesse.x, LimiteVitesse.y);
    }
}