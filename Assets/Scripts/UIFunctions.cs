using UnityEngine;
using UnityEngine.InputSystem;

public class UIFunctions : MonoBehaviour
{
    [SerializeField] InputAction tab, esc; // set variables for tab and esc buttons

    [SerializeField] GameObject menu; // assign quit menu gameobject

    [SerializeField] GameObject ui; // assign whole ui canvas 

    private void Awake()
    {
        tab.Enable();
        esc.Enable();

        esc.performed += i => {if(ui.activeInHierarchy) ToggleQuitMenu(); }; // Toggles the quit menu when esc is pressed
        tab.performed += i => {if(!menu.activeInHierarchy) ToggleUI(); }; // Toggles the UI when tab is pressed
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
