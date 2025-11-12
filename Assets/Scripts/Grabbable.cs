using UnityEngine;
using UnityEngine.InputSystem; // Unity New Input System
using System.Collections;

public class Grabbable : MonoBehaviour
{

    [SerializeField] protected InputAction leftClick, mousePos, scroll; // set variables for mouse click and mouse position, having these in this script is kinda messy, move to its own input action class later


    Vector3 currentScreenPos; // current screen position

    [SerializeField] float rotSpeed = 10f;
    Vector2 scrollRotation;

    Camera cam; // Access camera in scene
    bool isDragging;
    bool isRotating;

    protected bool isClickedOn 
    {
        get
        {
            Ray ray = cam.ScreenPointToRay(currentScreenPos); // cast a ray from current mouse position into the screen
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                return hit.transform == transform; // return true if ray hit something
            }
            return false; // return false if not 
        }
    }

    private Vector3 worldPos
    {
        get
        {
            float z = cam.WorldToScreenPoint(transform.position).z;
            return cam.ScreenToWorldPoint(currentScreenPos + new Vector3(0, 0, z));
        }
    }

    protected virtual void Awake()
    {
        cam = FindFirstObjectByType<Camera>(); // Find the camera in the scene

        // enable action bindings
        leftClick.Enable();
        mousePos.Enable();
        scroll.Enable();
        mousePos.performed += context => { currentScreenPos = context.ReadValue<Vector2>(); }; // Set current screen position to read mouse position
        scroll.performed += context => { scrollRotation = context.ReadValue<Vector2>(); }; // Read scroll wheel value
        scroll.canceled += i => { scrollRotation = Vector2.zero; }; // Reset scroll value to zero when not scrolling
        leftClick.performed += i => { if(isClickedOn) StartCoroutine(Drag()); /*StartCoroutine(Rotate());*/ }; // Run Drag() coroutine when left click is pressed
        leftClick.canceled += i => { isDragging = false; /*isRotating = false;*/ }; // When left click is released, stop dragging

        
        
    }

    IEnumerator Drag() // coroutine to run click and drag functionality
    {
        isDragging = true;

        Vector3 offset = transform.position - worldPos;

        while (isDragging) // while dragging
        {
            transform.position = worldPos + offset;

            scrollRotation *= rotSpeed;

            transform.Rotate(Vector3.up, scrollRotation.y, Space.World);

            yield return null; // Stop coroutine when not dragging
        }
    }

    /*
    IEnumerator Rotate() // coroutine to run rotate functionality
    {
        isRotating = true;

        while (isRotating)
        {
            // Rotation can only be performed while dragging on object

            scrollRotation *= rotSpeed;

            transform.Rotate(Vector3.up, scrollRotation.y, Space.World);

            yield return null;
        }
    }
    */
}
