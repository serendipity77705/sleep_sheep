using UnityEngine;
using System.Collections;

// This script goes on the objects being spawned (or gets added automatically)
public class MovingObject : MonoBehaviour
{
    private float speed;
    private float despawnX;
    private SideScrollSpawner spawner;
    private bool isSetup = false;

    public void SetupMovement(float moveSpeed, float despawnBoundary, SideScrollSpawner spawnerRef)
    {
        speed = moveSpeed;
        despawnX = despawnBoundary;
        spawner = spawnerRef;
        isSetup = true;
    }

    void Update()
    {
        if (!isSetup) return;

        // Move left
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Check if object has moved off the left side of screen
        if (transform.position.x < despawnX)
        {
            DespawnObject();
        }
    }

    private void DespawnObject()
    {
        // Notify spawner that this object is being destroyed
        if (spawner != null)
        {
            spawner.OnObjectDespawned();
        }

        Destroy(gameObject);
    }

    // Optional: Add this if you want objects to be destroyed when they hit something
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle collision with player (damage, etc.)
            // DespawnObject(); // Uncomment if you want object to disappear on hit
        }
    }

    // Safety check in case object gets destroyed by other means
    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnObjectDespawned();
        }
    }
}