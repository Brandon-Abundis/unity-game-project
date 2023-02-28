using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;

    Rigidbody playerRigidBody;

    public float movementSpeed = 7;

    private void Awake() {
        // placing the rigidbody on the same gameObject on the player locomotion, inputmanager script.
        inputManager = GetComponent<InputManager>();
        playerRigidBody = GetComponent<Rigidbody>();
    }
    public void HandleMovement() {
        // Up, Right = 1, Down, Left = -1.
        moveDirection = new Vector3(cameraObject.forward.x, 0f, cameraObject.forward.z) * inputManager.verticalInput;// the amount is = Movement input.
        // allows to move left and right based on horizontal input.
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize(); // keeps the circular direction of player to 1.
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        // move player based on previous calculations
        playerRigidBody.velocity = movementVelocity;
        
    }

    public void HandleRotation() {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        // Quaternion used to calculate rotations in unity.
        // where we are looking, is where we want to rotate.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        // 9:16

    }
}
