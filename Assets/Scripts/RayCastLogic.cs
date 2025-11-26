using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RayCastLogic : MonoBehaviour
{

    [SerializeField] GameObject sourceObject; // assign audio source
    [SerializeField] GameObject lineRendererObject; // assign line renderer gameobject

    Transform[] raycastTargets; // Raycast targets of audio source
    Transform[] raycastSources; // Raycast source of audio listener 

    List<float> initialSourcePositions; // List of initial raycast source positions for occlusion widening slider
    List<float> initialTargetPositions; // List of initial raycast target positions for occlusion widening slider

    LineRenderer[] lineRenderers; // list of line renderers to show debug lines

    [SerializeField] LayerMask occlusionLayer; // Assign walls to layer so that linecast only targets this layer

    int raycastHitCounter; // Counts number of rays intersecting with a wall
    int raycastAmount; // Counts the number of rays cast

    public float RayCastHitCounter { get { return raycastHitCounter; } } // public getter for raycastHitCounter

    public float RayCastAmount { get { return raycastAmount; } } // Returns the amount of raycasts cast

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raycastTargets = sourceObject.GetComponentsInChildren<Transform>(); // get the left, right, center targets of audio source
        raycastSources = GetComponentsInChildren<Transform>(); // Get the left, right, center targets of audio listener

        initialSourcePositions = new List<float>();
        initialTargetPositions = new List<float>();

        for (int i = 0; i < raycastSources.Length; i++) // Pre-count the amount of rays that need to be cast to instantiate line renderer game objects for debug function
        {
            initialSourcePositions.Add(raycastTargets[i].transform.position.x); // Add initial source positions to list.
            for (int j = 0; j < raycastTargets.Length; j++)
            {
                initialTargetPositions.Add(raycastTargets[j].transform.position.x); // Add initial source positions to list. 
                GameObject lineRenderer = Instantiate(lineRendererObject);
                lineRenderer.SetActive(false);
            }
        }

        lineRenderers = FindObjectsByType<LineRenderer>(FindObjectsInactive.Include,FindObjectsSortMode.InstanceID); // add all the instantiated line renderers to an array

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        raycastHitCounter = 0; // Resets raycast hit counter every tick
        raycastAmount = 0;
        PerformRaycasts();
    }

    void PerformRaycasts()
    {
        // Cycle between all raycast sources and raycast lengths, and then cast a ray from each of them
        for(int i = 0; i < raycastSources.Length; i++)
        {
            for(int j = 0; j < raycastTargets.Length; j++)
            {
                raycastAmount++; // increase the amount of rays cast by 1
                CastLine(raycastSources[i].transform.position, raycastTargets[j].transform.position, lineRenderers[raycastAmount-1]); // Cast the lines
            }
        }
    }

    void CastLine(Vector3 start, Vector3 end, LineRenderer lineRenderer) // Raycast logic
    {
        RaycastHit hit;
        Physics.Linecast(start, end, out hit, occlusionLayer); // cast out a line

        Vector3[] positions = new Vector3[2]; // Assign positions to a vector
        positions[0] = start;
        positions[1] = end;

        lineRenderer.positionCount = positions.Length; // Use positions list to get a count of points for the line renderer
        lineRenderer.SetPositions(positions); // Set the positions of the line renderers to the raycast positions

        if (hit.collider)
        {
            raycastHitCounter++;
            Debug.DrawLine(start, end, Color.red); // Display a red line when intersecting with an object, this is only for the editor version
            lineRenderer.startColor = Color.red; // These lines set colour for the line renderers in the build version
            lineRenderer.endColor = Color.red;
        }
        else
        {
            Debug.DrawLine(start, end, Color.green); // Display a green line when not intersecting with an object, this is only for the editor version
            lineRenderer.startColor = Color.green; // These lines set colour for the line renderers in the build version
            lineRenderer.endColor = Color.green;
        }
    }

    public void EnableDebug(Toggle toggle) // Public toggle function to enable debug visuals
    {
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].gameObject.SetActive(toggle.isOn);
        }
    }

    public void OcclusionWidthSlider(Slider slider) // Slider function for occlusion widening
    {
        for (int i = 0; i < raycastSources.Length; i++)
        {
            Transform sourceTransform = raycastSources[i].transform;

            sourceTransform.position = new Vector3(initialSourcePositions[i] * slider.value, sourceTransform.position.y, sourceTransform.position.z);

            for (int j = 0; j < raycastTargets.Length; j++)
            {
                Transform targetTransform = raycastTargets[j].transform;

                targetTransform.position = new Vector3(initialTargetPositions[j] * slider.value, targetTransform.position.y, targetTransform.position.z);
            }
        }
    }

}
