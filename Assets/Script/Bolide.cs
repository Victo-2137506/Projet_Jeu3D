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
    private float penaliteTemps = 2f;
    #endregion

    public UnityEvent evenementCollisionBarriere;

    private Vector2 controles;
    private float vitesseActuelle;
    private float dernierTempsCollision = -999f;
    private float delaiCollision = 3f;

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
    // Gère les collision contre les barrières pour avoir une pénalité de temps ( - 2 secondes)
    // Suggestion de Claude IA pour un temps de récupération après une pénalité
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Barriere"))
        {
            // Vérifie si assez de temps s'est écoulé depuis la dernière collision
            if (Time.time - dernierTempsCollision >= delaiCollision)
            {
                if (Chronometre.Instance != null)
                {
                    Chronometre.Instance.RetirerTemps(penaliteTemps);
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

    public IEnumerator AppliquerRalentissement(float multiplicateur, float duree)
    {
        float vitesseOriginale = vitesseActuelle;

        // Applique le multiplicateur
        vitesseActuelle *= multiplicateur;

        // Attendre la durée
        yield return new WaitForSeconds(duree);

        // Retour à la vitesse normale
        vitesseActuelle = Mathf.Clamp(vitesseOriginale, LimiteVitesse.x, LimiteVitesse.y);
    }
}