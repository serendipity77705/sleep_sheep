using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    public float moveDistance = 2f; // distance to move up or down

    void Start(){
        // Initialize sheep position or any other setup if needed
        Debug.Log("SheepMovement script started");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector3.down);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Vector3.up);
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

