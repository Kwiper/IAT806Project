using UnityEngine;
using UnityEngine.InputSystem; // Unity New Input System
using System.Collections;

public class Grabbable : MonoBehaviour
{

    [SerializeField] protected InputAction leftClick, mousePos, scroll; // set variables for mouse click and mouse position, having these in this script is kinda messy, move to its own input action class later


    Vector3 currentScreenPos; // current screen position

    [SerializeField] float rotSpeed = 10f; // Speed at which objects rotate when scrolling
    Vector2 scrollRotation; // Vector for rotation

    Camera cam; // Access camera in scene
    bool isDragging; // Check to see if clicking and dragging object

    Outline outline; // Outline component

    [SerializeField] Color highlightColour; // Set colours for outline
    [SerializeField] Color selectionColour; // Set colours for when object is grabbed

    protected bool isHoveredOn 
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
        leftClick.performed += i => { if(isHoveredOn) StartCoroutine(Drag()); /*StartCoroutine(Rotate());*/ }; // Run Drag() coroutine when left click is pressed
        leftClick.canceled += i => { isDragging = false; /*isRotating = false;*/ }; // When left click is released, stop dragging

        outline = gameObject.AddComponent<Outline>(); // Adds outline component to all grabbable objects programmatically so that it's not required to add it within editor

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = highlightColour;
        outline.OutlineWidth = 10f;

    }

    IEnumerator Drag() // coroutine to run click and drag functionality
    {
        isDragging = true;

        Vector3 offset = transform.position - worldPos;

        while (isDragging) // while dragging
        {
            transform.position = worldPos + offset;

            scrollRotation *= rotSpeed; // Set scroll rotation based on scroll wheel

            transform.Rotate(Vector3.up, scrollRotation.y, Space.World); // Rotate object

            yield return null; // Stop coroutine when not dragging
        }
    }

    private void Update()
    {
        // Enables/disables outline based on hover state 
        if (!isHoveredOn)
        {
            outline.enabled = false;
        }
        else
        {
            outline.enabled = true;

            if (isDragging)
            {
                outline.OutlineColor = selectionColour;
            }
            else
            {
                outline.OutlineColor = highlightColour;
            }
        }
    }

}
