using UnityEngine;

/// <summary>
/// Gère la lecture des effets sonores (Effets) du jeu.
/// Code inspiré des exercices de Jeu 2D : https://jeux2d-cegepvicto.github.io/EffetsSonores/
/// </summary>
public class Son : MonoBehaviour
{
    [SerializeField, Tooltip("AudioSource pour les effets sonores")]
    private AudioSource audioSource;

    [SerializeField, Tooltip("Son joué lors d'une collision avec une barrière")]
    private AudioClip sonCollisionBarriere;

    [SerializeField, Tooltip("Son joué quand un tour est complété")]
    private AudioClip sonTourComplete;

    /// <summary>
    /// Joue le son de collision avec une barrière.
    /// Appelé via l'événement Unity du script Bolide.
    /// </summary>
    public void JouerSonCollisionBarriere()
    {
        if (audioSource != null && sonCollisionBarriere != null)
        {
            audioSource.PlayOneShot(sonCollisionBarriere);
        }
        else
        {
            Debug.LogWarning("AudioSource ou AudioClip manquant pour le son de collision.");
        }
    }

    /// <summary>
    /// Joue le son quand un tour est complété.
    /// Appelé via l'événement Unity du script tour.
    /// </summary>
    public void JouerSonTourComplete()
    {
        if (audioSource != null && sonTourComplete != null)
        {
            audioSource.PlayOneShot(sonTourComplete);
        }
        else
        {
            Debug.LogWarning("AudioSource ou AudioClip manquant pour le son de tour complété.");
        }
    }

}