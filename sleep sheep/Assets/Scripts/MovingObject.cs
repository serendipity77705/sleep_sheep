using UnityEngine;
using System.Collections;

// This script goes on the objects being spawned (or gets added automatically)
public class MovingObject : MonoBehaviour
{
    private float speed;
    private float despawnX;
    private MultiItemSpawner spawner;
    private bool isSetup = false;
    private bool hasNotifiedSpawner = false; // Prevent multiple notifications

    public void SetupMovement(float moveSpeed, float despawnBoundary, MultiItemSpawner spawnerRef)
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
        NotifySpawnerOnce();
        Destroy(gameObject);
    }

    public void NotifySpawnerOfCollection()
    {
        NotifySpawnerOnce();
    }

    private void NotifySpawnerOnce()
    {
        if (!hasNotifiedSpawner && spawner != null)
        {
            hasNotifiedSpawner = true;
            spawner.OnObjectDespawned(gameObject);
        }
    }

    // Safety check in case object gets destroyed by other means
    void OnDestroy()
    {
        NotifySpawnerOnce();
    }
}