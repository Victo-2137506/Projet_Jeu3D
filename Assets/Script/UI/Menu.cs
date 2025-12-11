using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script de gestion du menu 
/// </summary>
public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    private bool siMenuOuvert = true;

    private void Awake()
    {
        Time.timeScale = 0.0f;
    }

    // Fonction appelée par l'action "Menu" dans le InputAction
    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleMenu();
        }
    }
    
    private void ToggleMenu()
    {
        if (siMenuOuvert)
            FermerMenu();
        else
            OuvrirMenu();
    }

    /// <summary>
    /// Ouvre le menu
    /// </summary>
    public void OuvrirMenu()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0f;
        siMenuOuvert = true;
    }

    /// <summary>
    /// Ferme le menu
    /// </summary>
    public void FermerMenu()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        siMenuOuvert = false;
    }

}
