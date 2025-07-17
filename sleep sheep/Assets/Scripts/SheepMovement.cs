using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    public float moveDistance = 0.5f; // distance to move up or down

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
}
