using UnityEngine;
using UnityEngine.InputSystem;

public class UIFunctions : MonoBehaviour
{
    [SerializeField] InputAction tab, esc; // set variables for tab and esc buttons

    [SerializeField] GameObject menu;

    private void Awake()
    {
        tab.Enable();
        esc.Enable();

        esc.performed += i => { ToggleQuitMenu(); };
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleQuitMenu()
    {
        menu.SetActive(!menu.activeInHierarchy);
    }

}
