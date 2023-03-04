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
    public float rotationSpeed = 15;

    private void Awake() {
        // placing the rigidbody on the same gameObject on the player locomotion, inputmanager script.
        inputManager = GetComponent<InputManager>();
        playerRigidBody = GetComponent<Rigidbody>();
        // will scan the scene for the thing that was tagged 'main camera'.
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement() {
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement() {
        // Up, Right = 1, Down, Left = -1.
        moveDirection = new Vector3(cameraObject.forward.x, 0f, cameraObject.forward.z) * inputManager.verticalInput;// the amount is = Movement input.
        // moveDirection = cameraObject.forward * inputManager.horizontalInput;
        // allows to move left and right based on horizontal input.
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize(); // keeps the circular direction of player to 1.
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        // move player based on previous calculations.
        playerRigidBody.velocity = movementVelocity;
        
    }

    private void HandleRotation() {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        // keep the rotation at the position we are looking when stopped
        if (targetDirection == Vector3.zero) {
            targetDirection = transform.forward;
        }

        // Quaternion used to calculate rotations in unity.
        // where we are looking, is where we want to rotate.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        // rotation between point a and b.
        // use current rotation and the new rotation from movement keys.
        // time.deltatime constant variable change to the framerate.
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;

    }
}
