using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject player;
    bool followPlayer = true;
    PlayerMovement pm;
    Camera cam;
    /*int camLockBackSpeed = new Thread.Sleep(3000);*/
    float followSpeed = 5.0f; // Speed for following the player
    float lookAheadSpeed = 8.0f; // Speed for looking ahead
    float edgeMargin = 0.05f; // Margin for the edges of the screen

    Vector3 lookAheadOffset = Vector3.zero; // Offset for the look-ahead functionality

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pm = player.GetComponent<PlayerMovement>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            followPlayer = false;
            LookAhead();
        }
        else
        {
            followPlayer = true;
            FollowPlayer();
        }
    }

    // Public method to set followPlayer value
    public void SetFollowPlayer(bool val)
    {
        followPlayer = val;
    }

    // Method to follow the player
    void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }

    // Method to look ahead
    void LookAhead()
    {
        if (player == null) return;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);

        // Calculate the look-ahead position, but clamp it to prevent excessive movement
        Vector3 lookAheadPos = targetPos + (mouseWorldPos - targetPos) * 0.5f; // Adjust factor as needed

        // Clamp the look-ahead position to prevent the camera from moving too far away
        float clampedX = Mathf.Clamp(lookAheadPos.x, targetPos.x - 3.0f, targetPos.x + 3.0f); // Adjust clamp values as needed
        float clampedY = Mathf.Clamp(lookAheadPos.y, targetPos.y - 3.0f, targetPos.y + 3.0f); // Adjust clamp values as needed

        lookAheadPos = new Vector3(clampedX, clampedY, this.transform.position.z);

        // If the player is at the edge, follow the player closely
        if (IsPlayerAtScreenEdge())
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, followSpeed * Time.deltaTime);
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, lookAheadPos, lookAheadSpeed * Time.deltaTime);
        }
    }

    // Check if player is at the edge of the screen
    bool IsPlayerAtScreenEdge()
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(player.transform.position);
        return viewportPoint.x <= edgeMargin || viewportPoint.x >= (1 - edgeMargin) ||
               viewportPoint.y <= edgeMargin || viewportPoint.y >= (1 - edgeMargin);
    }
}
