using UnityEngine;

/// <summary>
/// Gère les écrans de fin de partie.
/// </summary>
public class GestionFinDePartie : MonoBehaviour
{
    public GameObject menuVictoire;
    public GameObject menuDefaite;

    // Appelé quand le joueur gagne
    public void AfficherVictoire()
    {
        menuVictoire.SetActive(true);
        Time.timeScale = 0f;
    }

    // Appelé quand le joueur perd
    public void AfficherDefaite()
    {
        menuDefaite.SetActive(true);
        Time.timeScale = 0f;
    }

    // Appelé quand le joueur clique sur le bouton "Quitter"
    public void QuitterJeu()
    {
        Application.Quit();
    }
}
