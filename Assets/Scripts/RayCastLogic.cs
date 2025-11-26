using UnityEngine;

public class RayCastLogic : MonoBehaviour
{

    [SerializeField] GameObject sourceObject; // assign audio source

    Transform[] raycastTargets; // Raycast targets of audio source
    Transform[] raycastSources; // Raycast source of audio listener 

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
                CastLine(raycastSources[i].transform.position, raycastTargets[j].transform.position);
                raycastAmount++;
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
