using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCursor : MonoBehaviour
{
    Camera cam;
    Rigidbody2D rb;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D from the same GameObject as this script
    }

    void Update()
    {
        RotatePlayerToCursor();
    }

    void RotatePlayerToCursor()
    {
        // Get mouse position in world space
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Calculate direction from player's position to mouse position
        Vector2 direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        // Calculate angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation directly to transform.rotation around Z-axis
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // Adjust for sprite's default facing direction
    }
}
