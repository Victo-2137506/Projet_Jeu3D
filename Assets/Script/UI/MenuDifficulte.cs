using UnityEngine;

/// <summary>
/// Script qui gère les configurations de difficulté
/// Code inspiré de : https://learn.unity.com/course/create-with-code/unit/unit-5-user-interface/tutorial/lesson-5-4-what-s-the-difficulty-2?version=6.0
/// </summary>
public class MenuDifficulte : MonoBehaviour
{
    // Singleton pour accès facile, mais il est détruit avec la scène.
    public static MenuDifficulte Instance { get; private set; }

    [SerializeField] private GameObject menuDifficulte;
    public ConfigChrono ConfigChrono { get; private set; }
    public Ralentissement Ralentissement { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (menuDifficulte != null)
        {
            menuDifficulte.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    /// <summary>
    /// Méthode que BoutonDifficulte va utilisé pour sélectionner les instances correspondantes.
    /// </summary>
    /// <param name="chrono"></param>
    /// <param name="ralenti"></param>
    public void ChoisirConfiguration(ConfigChrono chrono, Ralentissement ralenti)
    {
        ConfigChrono = chrono;
        Ralentissement = ralenti;

        // Initialisation du Chronomètre avant la reprise du jeu
        if (Chronometre.Instance != null)
        {
            Chronometre.Instance.InitialiserChrono(ConfigChrono);
        }

        if (menuDifficulte != null)
        {
            menuDifficulte.SetActive(false);
        }
    }
}