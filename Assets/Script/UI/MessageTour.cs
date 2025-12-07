using System.Collections;
using TMPro;
using UnityEngine;

public class MessageTour : MonoBehaviour
{
    public static MessageTour Instance { get; private set; }

    private float dureeMessage = 4f;
    private Coroutine routineAffichage;

    [Header("Référence UI")]
    [SerializeField] private TextMeshProUGUI texteMessage;
    [SerializeField] private GameObject conteneurMessage;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Affiche le texte déjà défini dans l’inspecteur (texteMessage.text).
    /// </summary>
    public void AfficherMessage()
    {
        if (routineAffichage != null)
            StopCoroutine(routineAffichage);

        conteneurMessage.SetActive(true);
        animator.SetTrigger("Apparition");

        routineAffichage = StartCoroutine(CacherApresDelai());
    }

    private IEnumerator CacherApresDelai()
    {
        yield return new WaitForSeconds(dureeMessage);
        animator.SetTrigger("Disparition");
        routineAffichage = null;
    }

    public void FinAnimation()
    {
        conteneurMessage.SetActive(false);
    }
}
