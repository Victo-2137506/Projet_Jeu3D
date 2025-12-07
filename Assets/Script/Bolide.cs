using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script de contrôle pour un véhicule de course (bolide) :
/// accélération, freinage, rotation dépendante de la vitesse.
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

    [field: SerializeField, Tooltip("Force de freinage lorsqu’on appuie en arrière")]
    public float Freinage { get; private set; }

    [field: SerializeField, Tooltip("Vitesse de rotation du véhicule")]
    public float VitesseVirage { get; private set; }

    [field: SerializeField, Tooltip("Facteur de virage selon la vitesse (x=min, y=max)")]
    public Vector2 FacteurTauxVirage { get; private set; }
    #endregion

    #region Variables internes
    private Vector2 controles;
    private float vitesseActuelle;
    #endregion

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
    /// Reçoit les entrées du joueur (Input System).
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

        // Petites vitesses = arrêt
        if (Mathf.Abs(vitesseActuelle) < 0.01f)
            vitesseActuelle = 0f;

        // Déplacement
        transform.position += transform.forward * vitesseActuelle * Time.deltaTime;

        // Si le chronomètre n'existe pas
        if (!Chronometre.Instance) return;

        // Dès que le véhicule bouge lance le chronomètre
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

        // Facteur de virage selon la vitesse (version plus stable)
        float t = Mathf.InverseLerp(0f, LimiteVitesse.y, Mathf.Abs(vitesseActuelle));
        float facteurVirage = Mathf.Lerp(FacteurTauxVirage.x, FacteurTauxVirage.y, t);

        transform.Rotate(
            Vector3.up,
            inputRotation * VitesseVirage * facteurVirage * Time.deltaTime
        );
    }
    #endregion

public IEnumerator AppliquerModificateurVitesseProgressif(float multiplicateur, float duree)
{
    float vitesseInitiale = vitesseActuelle;
    float vitesseCible = Mathf.Clamp(vitesseActuelle * multiplicateur, LimiteVitesse.x, LimiteVitesse.y);

    float temps = 0f;
    while (temps < duree)
    {
        temps += Time.deltaTime;
        // Interpolation progressive
        vitesseActuelle = Mathf.Lerp(vitesseInitiale, vitesseCible, temps / duree);
        yield return null;
    }

    // Optionnel : retour à la vitesse normale après la durée
    yield return new WaitForSeconds(1f);
    vitesseActuelle = Mathf.Clamp(vitesseInitiale, LimiteVitesse.x, LimiteVitesse.y);
}


}
