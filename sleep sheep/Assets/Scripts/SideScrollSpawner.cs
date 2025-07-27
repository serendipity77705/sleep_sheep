using UnityEngine;
using System.Collections;

// This script goes on the spawner GameObject
public class SideScrollSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject objectToSpawn;
    public float delayAfterDespawn = 1f;      // Wait time after object despawns
    public bool autoSpawn = true;
    [SerializeField] private float minInitialDelay = 4f;  // Minimum initial spawn delay
    [SerializeField] private float maxInitialDelay = 10f; // Maximum initial spawn delay
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;              // Speed objects move left
    
    [Header("Spawn Position")]
    public float spawnOffsetX = 1f;           // How far off-screen to spawn
    public bool randomizeY = true;            // Random Y position
    public float minY = -4f;                  // Min Y spawn position
    public float maxY = 4f;                   // Max Y spawn position
    
    private Camera cam;
    private float screenRightEdge;
    private float screenLeftEdge;
    private GameObject currentObject;         // Track the current spawned object
    
    void Start()
    {
        cam = Camera.main;
        CalculateScreenBounds();
        
        if (autoSpawn)
        {
            float randomDelay = Random.Range(minInitialDelay, maxInitialDelay);
            StartCoroutine(DelayedSpawnStart(randomDelay));
        }
    }
    
    void CalculateScreenBounds()
    {
        // Calculate screen edges in world coordinates
        Vector3 rightEdge = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, cam.nearClipPlane));
        Vector3 leftEdge = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        
        screenRightEdge = rightEdge.x;
        screenLeftEdge = leftEdge.x;
    }

    IEnumerator DelayedSpawnStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnRoutine());
    }
    
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Only spawn if no object currently exists
            if (currentObject == null)
            {
                SpawnObject();
            }
            
            // Wait for current object to be destroyed
            yield return new WaitUntil(() => currentObject == null);
            
            // Wait additional time after despawn
            yield return new WaitForSeconds(delayAfterDespawn);
        }
    }
    
    public void SpawnObject()
    {
        if (objectToSpawn == null) return;
        
        // Calculate spawn position (just off the right side of screen)
        Vector3 spawnPos = new Vector3(
            screenRightEdge + spawnOffsetX,
            randomizeY ? Random.Range(minY, maxY) : 0f,
            0f
        );
        
        // Spawn the object and keep reference to it
        currentObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        
        // Add the movement component
        MovingObject movingComponent = currentObject.GetComponent<MovingObject>();
        if (movingComponent == null)
        {
            movingComponent = currentObject.AddComponent<MovingObject>();
        }
        
        // Set the movement speed and despawn boundary, and pass reference to spawner
        movingComponent.SetupMovement(moveSpeed, screenLeftEdge - 2f, this);
    }
    
    // Called by MovingObject when it gets destroyed
    public void OnObjectDespawned()
    {
        currentObject = null;
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw spawn area in scene view
        if (Application.isPlaying && cam != null)
        {
            Gizmos.color = Color.green;
            Vector3 spawnLine = new Vector3(screenRightEdge + spawnOffsetX, 0, 0);
            Gizmos.DrawLine(spawnLine + Vector3.up * 10, spawnLine + Vector3.down * 10);
            
            Gizmos.color = Color.red;
            Vector3 despawnLine = new Vector3(screenLeftEdge - 2f, 0, 0);
            Gizmos.DrawLine(despawnLine + Vector3.up * 10, despawnLine + Vector3.down * 10);
        }
    }
}