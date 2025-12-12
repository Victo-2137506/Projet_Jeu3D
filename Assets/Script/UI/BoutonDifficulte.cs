using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script qui gère la difficulté des boutons sélectionnés
/// Code inspiré de : https://learn.unity.com/course/create-with-code/unit/unit-5-user-interface/tutorial/lesson-5-4-what-s-the-difficulty-2?version=6.0
/// </summary>
public class BoutonDifficulte : MonoBehaviour
{
    private Button bouton;
    private MenuDifficulte difficultyManager;

    [SerializeField] 
    private ConfigChrono ConfigChrono;
    [SerializeField] 
    private Ralentissement ConfigRalentissement;

    public void Start()
    {
        bouton = GetComponent<Button>();

        difficultyManager = MenuDifficulte.Instance;

        bouton.onClick.AddListener(DefinirDifficulte);
    }

    /// <summary>
    /// Appelé quand le bouton est cliqué
    /// </summary>
    public void DefinirDifficulte()
    {
        // Prend les configuration des instances des ScriptableObjects
        difficultyManager.ChoisirConfiguration(ConfigChrono, ConfigRalentissement);
    }
}