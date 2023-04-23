using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPartnerPosititon : MonoBehaviour
{
public Transform playerTransform;   // Reference to the player transform
    public Transform cameraTransform;   // Reference to the camera transform
    public float distance = 2.0f;       // Distance from player to floating object
    public float height = 1.0f;         // Height of the floating object

    void Update()
    {
        // Calculate the rotation angle around the player based on the camera position
        float rotationAngle = cameraTransform.eulerAngles.y;

        // Set the position of the floating object relative to the player
        Vector3 offset = Quaternion.Euler(0, rotationAngle, 0) * new Vector3(0, height, -distance);
        transform.position = playerTransform.position + offset;

        // Rotate the floating object to face the player
        transform.LookAt(playerTransform);
    }
}
