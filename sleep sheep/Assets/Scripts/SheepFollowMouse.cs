using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField]
    private float maxSpeed = 15f;
    private float stopDistance = 3f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        FollowMousePositionWithStop(maxSpeed);
    }

    private void FollowMousePositionWithStop(float maxSpeed)
    {
        Vector2 mousePos = GetWorldPositionFromMouse();
        float distance = Vector2.Distance(transform.position, mousePos);

        if (distance > stopDistance)
        {
            // Only move if the sheep is farther than stopDistance
            transform.position = Vector2.MoveTowards(transform.position, mousePos,
                maxSpeed * Time.deltaTime);
        }
        // Else, don't move — sheep stops and leaves space for the button
    }

    private Vector2 GetWorldPositionFromMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}