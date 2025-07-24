using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    private float moveDistance = 1f; // distance to move up or down
    private int currentStep = 0;
    private const int MAX_STEPS = 3;

    void Start(){
        // Initialize sheep position or any other setup if needed
        Debug.Log("Starting position: " + transform.position);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && currentStep > -MAX_STEPS)
        {
            Move(Vector3.down);
            currentStep--;
            Debug.Log($"Current step: {currentStep}");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && currentStep < MAX_STEPS)
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
        if (collision.gameObject.CompareTag("Good")) {
            Debug.Log("Collision with good object detected. Good Job!");
            Destroy(collision.gameObject);
        } 
        else if (collision.gameObject.CompareTag("Bad")) {
            Debug.Log("Collision with bad object detected. Oh no!");
            Destroy(collision.gameObject);
        }
    }

}

