using UnityEngine;
using UnityEngine.InputSystem;

public class UIFunctions : MonoBehaviour
{
    [SerializeField] InputAction tab, esc; // set variables for tab and esc buttons

    [SerializeField] GameObject menu;

    [SerializeField] GameObject ui;

    private void Awake()
    {
        tab.Enable();
        esc.Enable();

        esc.performed += i => {if(ui.activeInHierarchy) ToggleQuitMenu(); };
        tab.performed += i => {if(!menu.activeInHierarchy) ToggleUI(); };
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleQuitMenu()
    {
        menu.SetActive(!menu.activeInHierarchy);
    }

    void ToggleUI()
    {
        ui.SetActive(!ui.activeInHierarchy);
    }

}
