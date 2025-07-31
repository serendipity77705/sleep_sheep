using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum SpawnerType
{
    Good,
    Bad
}

public class MultiItemSpawner : MonoBehaviour
{
    [Header("Spawner Type")]
    public SpawnerType spawnerType = SpawnerType.Good;
    
    [Header("Item Lists")]
    public GameObject[] goodItems;            // List of good item prefabs
    public GameObject[] badItems;             // List of bad item prefabs
    
    [Header("Spawn Settings")]
    public float minSpawnInterval = 2f;       // Minimum time between spawns
    public float maxSpawnInterval = 5f;       // Maximum time between spawns
    public bool autoSpawn = true;
    [SerializeField] private float minInitialDelay = 2f;  // Minimum initial spawn delay
    [SerializeField] private float maxInitialDelay = 10f; // Maximum initial spawn delay
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;              // Speed objects move left
    
    [Header("Spawn Position")]
    public float spawnOffsetX = 1f;           // How far off-screen to spawn
    public bool randomizeY = true;            // Random Y position
    public float minY = -4f;                  // Min Y spawn position
    public float maxY = 4f;                   // Max Y spawn position
    
    [Header("Win Condition")]
    public string winSceneName = "WinScreen";  // Name of the win scene
    public float winDelay = 2f;               // Delay before loading win scene
    
    private Camera cam;
    private float screenRightEdge;
    private float screenLeftEdge;
    private List<GameObject> activeObjects = new List<GameObject>(); // Track multiple active objects
    private float[] spawnYPositions = new float[] 
    { 
        3.12f,  // Top
        2.12f,
        1.12f,
        0.12f,  // Middle
        -0.88f,
        -1.88f,
        -2.88f  // Bottom
    };
    
    // Static list to track collected good items across all spawners
    private static HashSet<GameObject> collectedGoodItems = new HashSet<GameObject>();
    
    // Static reference to track total good items across all spawners
    private static int totalGoodItems = 0;
    private static bool winSceneLoading = false;
    
    void Start()
    {
        cam = Camera.main;
        CalculateScreenBounds();
        
        // Count total good items if this is a good spawner
        if (spawnerType == SpawnerType.Good)
        {
            totalGoodItems += goodItems.Length;
        }
        
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
            // Clean up destroyed objects from the list
            activeObjects.RemoveAll(obj => obj == null);
            
            GameObject objectToSpawn = GetItemToSpawn();
            if (objectToSpawn != null)
            {
                SpawnObject(objectToSpawn);
            }
            else if (spawnerType == SpawnerType.Good)
            {
                Debug.Log("No more good items to spawn - all collected!");
                yield break; // Stop spawning good items
            }
            
            // Wait random interval before next spawn
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    
    public void SpawnObject(GameObject objectToSpawn)
    {
        if (objectToSpawn == null) return;
        
        // Get random Y position from array
        float randomY = spawnYPositions[Random.Range(0, spawnYPositions.Length)];
        
        // Calculate spawn position (just off the right side of screen)
        Vector3 spawnPos = new Vector3(
            screenRightEdge + spawnOffsetX,
            randomY,
            0f
        );
        
        // Spawn the object and add to active objects list
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        activeObjects.Add(spawnedObject);
        
        // Add ItemIdentifier to track original prefab (for good items)
        if (spawnerType == SpawnerType.Good)
        {
            ItemIdentifier itemId = spawnedObject.GetComponent<ItemIdentifier>();
            if (itemId == null)
            {
                itemId = spawnedObject.AddComponent<ItemIdentifier>();
            }
            itemId.SetOriginalPrefab(objectToSpawn);
        }
        
        // Add the movement component
        MovingObject movingComponent = spawnedObject.GetComponent<MovingObject>();
        if (movingComponent == null)
        {
            movingComponent = spawnedObject.AddComponent<MovingObject>();
        }
        
        // Set the movement speed and despawn boundary, and pass reference to spawner
        movingComponent.SetupMovement(moveSpeed, screenLeftEdge - 2f, this);
    }
    
    GameObject GetItemToSpawn()
    {
        if (spawnerType == SpawnerType.Bad)
        {
            // For bad items, just pick randomly from the list
            if (badItems.Length == 0) return null;
            return badItems[Random.Range(0, badItems.Length)];
        }
        else // Good items
        {
            // Get uncollected good items
            List<GameObject> uncollectedItems = goodItems.Where(item => !collectedGoodItems.Contains(item)).ToList();
            
            if (uncollectedItems.Count == 0)
            {
                Debug.Log("All good items have been collected!");
                return null; // No more items to spawn
            }
            
            // Pick randomly from uncollected items
            return uncollectedItems[Random.Range(0, uncollectedItems.Count)];
        }
    }
    
    // Call this when a good item is collected (from your SheepMovement script)
    public static void MarkGoodItemAsCollected(GameObject itemPrefab)
    {
        collectedGoodItems.Add(itemPrefab);
        Debug.Log($"Good item {itemPrefab.name} marked as collected. Total collected: {collectedGoodItems.Count}/{totalGoodItems}");
        
        // Check if all good items have been collected
        if (collectedGoodItems.Count >= totalGoodItems && !winSceneLoading)
        {
            Debug.Log("All good items collected! Loading WinScene...");
            LoadWinScene();
        }
    }
    
    // Load the win scene
    private static void LoadWinScene()
    {
        winSceneLoading = true;
        
        // Find a spawner to get the win scene name and delay
        MultiItemSpawner spawner = FindObjectOfType<MultiItemSpawner>();
        if (spawner != null)
        {
            spawner.StartCoroutine(spawner.LoadWinSceneCoroutine());
        }
        else
        {
            // Fallback if no spawner found
            SceneManager.LoadScene("WinScreen");
        }
    }
    
    private IEnumerator LoadWinSceneCoroutine()
    {
        Debug.Log($"Loading {winSceneName} in {winDelay} seconds...");
        yield return new WaitForSeconds(winDelay);
        SceneManager.LoadScene(winSceneName);
    }
    
    // Call this to reset collected items (for new game/level restart)
    public static void ResetCollectedItems()
    {
        collectedGoodItems.Clear();
        totalGoodItems = 0;
        winSceneLoading = false;
        Debug.Log("Collected items list reset");
    }
    
    // Check if all good items have been collected
    public bool AllGoodItemsCollected()
    {
        return collectedGoodItems.Count >= totalGoodItems;
    }
    
    // Get total number of good items across all spawners
    public static int GetTotalGoodItems()
    {
        return totalGoodItems;
    }
    
    // Get number of collected good items
    public static int GetCollectedGoodItemsCount()
    {
        return collectedGoodItems.Count;
    }
    
    // Get list of remaining uncollected good items
    public List<GameObject> GetUncollectedGoodItems()
    {
        if (spawnerType != SpawnerType.Good) return new List<GameObject>();
        return goodItems.Where(item => !collectedGoodItems.Contains(item)).ToList();
    }
    
    // Called by MovingObject when it gets destroyed or collected
    public void OnObjectDespawned(GameObject obj)
    {
        activeObjects.Remove(obj);
    }
    
    // Get count of currently active objects
    public int GetActiveObjectCount()
    {
        activeObjects.RemoveAll(obj => obj == null);
        return activeObjects.Count;
    }
    
    // Force spawn a specific item (useful for testing)
    public void ForceSpawnItem(GameObject itemPrefab)
    {
        if (itemPrefab != null)
        {
            SpawnObject(itemPrefab);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw spawn area in scene view
        if (Application.isPlaying && cam != null)
        {
            Gizmos.color = spawnerType == SpawnerType.Good ? Color.green : Color.red;
            Vector3 spawnLine = new Vector3(screenRightEdge + spawnOffsetX, 0, 0);
            Gizmos.DrawLine(spawnLine + Vector3.up * 10, spawnLine + Vector3.down * 10);
            
            Gizmos.color = Color.yellow;
            Vector3 despawnLine = new Vector3(screenLeftEdge - 2f, 0, 0);
            Gizmos.DrawLine(despawnLine + Vector3.up * 10, despawnLine + Vector3.down * 10);
        }
        
        // Draw different colored gizmos for good vs bad spawners
        Gizmos.color = spawnerType == SpawnerType.Good ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}