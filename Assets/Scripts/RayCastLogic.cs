using UnityEngine;

public class RayCastLogic : MonoBehaviour
{

    [SerializeField] GameObject sourceObject; // assign audio source

    Transform[] raycastTargets; // Raycast targets of audio source
    Transform[] raycastSources; // Raycast source of audio listener 

    [SerializeField] LayerMask occlusionLayer; // Assign walls to layer so that linecast only targets this layer

    int raycastHitCounter; // Counts number of rays intersecting with a wall

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raycastTargets = sourceObject.GetComponentsInChildren<Transform>(); // get the left, right, center targets of audio source
        raycastSources = GetComponentsInChildren<Transform>(); // Get the left, right, center targets of audio listener

        /* // debug message for getting the raycast targets and sources in the array to make sure it's working properly
        for(int i = 0; i < raycastTargets.Length; i++)
        {
            Debug.Log(raycastTargets[i]);
        }

        for(int i = 0; i < raycastSources.Length; i++)
        {
            Debug.Log(raycastSources[i]);
        }
        */ 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OccludeBetween();
        raycastHitCounter = 0; // Resets raycast hit counter every tick
    }

    void OccludeBetween()
    {
        // Cycle between all raycast sources and raycast lengths, and then cast a ray from each of them
        for(int i = 0; i < raycastSources.Length; i++)
        {
            for(int j = 0; j < raycastTargets.Length; j++)
            {
                CastLine(raycastSources[i].transform.position, raycastTargets[j].transform.position);
            }
        }
    }

    void CastLine(Vector3 start, Vector3 end) // Raycast logic
    {
        RaycastHit hit;
        Physics.Linecast(start, end, out hit, occlusionLayer);

        if (hit.collider)
        {
            raycastHitCounter++;
            Debug.DrawLine(start, end, Color.red); // Display a red line when intersecting with an object
        }
        else
        {
            Debug.DrawLine(start, end, Color.green); // Display a green line when not intersecting with an object
        }
    }
}
