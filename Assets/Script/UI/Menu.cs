using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    private bool estOuvertMenu = false;

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (estOuvertMenu)
            FermerMenu();
        else
            OuvertMenu();
    }

    public void OuvertMenu()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0f;
        estOuvertMenu = true;
    }

    public void FermerMenu()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        estOuvertMenu = false;
    }
}
