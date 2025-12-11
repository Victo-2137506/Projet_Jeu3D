using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script qui s'occupe du défilement du texte.
/// </summary>
public class DefilementTexte : MonoBehaviour
{
    public float speed = 100f;
    private RectTransform rectTransform;
    private bool defilementActif = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!defilementActif) return;

        rectTransform.anchoredPosition += Vector2.right * speed * Time.deltaTime;
    }

    /// <summary>
    /// Active le défilement du texte
    /// </summary>
    public void ActiverDefilement()
    {
        defilementActif = true;

        // Remet la position à zéro pour recommencer au début le texte
        rectTransform.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// Désactive le défilement du texte
    /// </summary>
    public void DesactiverDefilement()
    {
        defilementActif = false;
    }
}