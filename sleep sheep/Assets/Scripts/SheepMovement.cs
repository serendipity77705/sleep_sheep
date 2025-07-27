using UnityEngine;
using System.Collections.Generic;

public class SheepMovement : MonoBehaviour
{
    private float moveDistance = 1f; // distance to move up or down
    private int currentStep = 0;
    private const int MAX_STEPS = 3;

    private Vector2 startPosGood = new Vector2(0f, 4f); // Starting point for good items
    private Vector2 startPosBad = new Vector2(0f, -3f);  // Starting point for bad items
    private float itemSpacing = 0.5f; // Space between collected items

    private int goodItemCount = 0;
    private int badItemCount = 0;
    private int totalItemCount = 0;

    private Vector2 collectedItemLocation = new Vector2(-6.5f, -4.5f);

    void Start(){
        Debug.Log("Starting position: " + transform.position);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && currentStep > -MAX_STEPS)
        {
            Move(Vector3.down);
            currentStep--;
            Debug.Log($"Current step: {currentStep}");
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && currentStep < MAX_STEPS)
        {
            Move(Vector3.up);
            currentStep++;
            Debug.Log($"Current step: {currentStep}");
        }
    }

    void Move(Vector3 direction)
    {
        transform.position += direction * moveDistance;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject item = collision.gameObject;

        if (item.CompareTag("Good")) {
            Debug.Log("Collision with good object detected. Good Job!");
            CollectItem(item, collectedItemLocation + new Vector2(itemSpacing * totalItemCount, 0));
            goodItemCount++;
        } 
        else if (item.CompareTag("Bad")) {
            Debug.Log("Collision with bad object detected. Oh no!");
            CollectItem(item, collectedItemLocation + new Vector2(itemSpacing * totalItemCount, 0));
            badItemCount++;
        }
    }

    void CollectItem(GameObject item, Vector2 newPosition)
    {
        totalItemCount = goodItemCount + badItemCount;

        // Move to the corner
        if (item.CompareTag("Good"))
        {
            // Shrink item
            item.transform.localScale = new Vector2(0.75f, 0.75f);
            // Move to collection area
            item.transform.position = newPosition;
            // Disable MovingObject script for collectible items
            MovingObject movingScript = item.GetComponent<MovingObject>();
            if (movingScript != null)
            {
                movingScript.enabled = false;
            }
        }
        else if (item.CompareTag("Bad"))
        {
            Destroy(item);
            return;
        }
        // Disable future collisions
        Collider2D col = item.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Stop physics simulation
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;
    }
}
