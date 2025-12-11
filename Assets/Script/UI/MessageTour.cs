using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Script pour l'affichage du message de fin de tour.
/// Code inspiré du demo-rpg.
/// </summary>
public class MessageTour : MonoBehaviour
{
    public static MessageTour Instance { get; private set; }

    private float dureeMessage = 4f;
    private Coroutine routineAffichage;

    [Header("Référence UI")]
    [SerializeField] private TextMeshProUGUI texteMessage;
    [SerializeField] private GameObject conteneurMessage;
    [SerializeField] private Animator animator;
    [SerializeField] private DefilementTexte defilement;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Affiche le texte
    /// </summary>
    public void AfficherMessage()
    {
        if (routineAffichage != null)
            StopCoroutine(routineAffichage);

        conteneurMessage.SetActive(true);
        animator.SetTrigger("Apparition");
        defilement.ActiverDefilement();

        routineAffichage = StartCoroutine(CacherMessage());
    }

    /// <summary>
    /// Cache le texte après un certain délais
    /// </summary>
    /// <returns></returns>
    private IEnumerator CacherMessage()
    {
        yield return new WaitForSeconds(dureeMessage);
        animator.SetTrigger("Disparition");
        routineAffichage = null;
        defilement.DesactiverDefilement();

    }

    /// <summary>
    /// Met fin à l'animation
    /// </summary>
    public void FinAnimation()
    {
        conteneurMessage.SetActive(false);
    }
}
