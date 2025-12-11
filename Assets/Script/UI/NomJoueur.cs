using UnityEngine;
using TMPro;

/// <summary>
/// Permet d'afficher le nom du joueur
/// </summary>
public class NomJoueur : MonoBehaviour
{
    public TMP_InputField champPrenom;
    public TMP_Text texteAffiche;
    private int limiteCaracteres = 20;

    void Start()
    {
        // Met à jour le texte dès que l'utilisateur tape quelque chose
        champPrenom.onValueChanged.AddListener(MettreAJourTexte);

        // Limite de caractères
        champPrenom.characterLimit = limiteCaracteres;
    }

    private void MettreAJourTexte(string nouveauTexte)
    {
        texteAffiche.text = nouveauTexte;
    }
}
