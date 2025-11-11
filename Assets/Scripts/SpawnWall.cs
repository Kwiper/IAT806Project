using UnityEngine;

public class SpawnWall : MonoBehaviour
{

    [SerializeField] GameObject wall; // store wall prefab
     
    [SerializeField] Transform spawnPoint; // Store position to spawn wall, might change this so the wall just moves around when you press the button until you click to instantiate it but doesn't matter right now 

    Vector3 spawnPosition;

    private void Awake()
    {
        spawnPosition = spawnPoint.transform.position;
    }

    public void Spawn()
    {
        Instantiate(wall, spawnPosition, Quaternion.identity);
    }
}
