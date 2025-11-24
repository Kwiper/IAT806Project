using UnityEngine;
using UnityEngine.InputSystem;

public class Deletable : Grabbable // Inherit from Grabbable
{
    [SerializeField] InputAction rightClick; // Add right click function ONLY to Deletable objects

    protected override void Awake()
    {
        base.Awake();

        SaveSystem.objects.Add(this); // Add object to save system list for loading

        rightClick.Enable();
        rightClick.performed += i => { if (isHoveredOn) DestroySelf(); }; // Destroy object on right click
    }

    private void OnDisable() // Disable inputs when object is destroyed
    {
        leftClick.Disable();
        mousePos.Disable();
        rightClick.Disable();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SaveSystem.objects.Remove(this); // Remove object from save file list when destroyed
    }
}
