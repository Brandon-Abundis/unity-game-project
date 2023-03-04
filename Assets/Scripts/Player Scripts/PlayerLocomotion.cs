using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;

    Rigidbody playerRigidBody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastOriginHeight = 0.5f;
    public float maxDistance = 1;
    public LayerMask groundLayer;
    

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;

    [Header("Movenment Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    private void Awake() {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        // placing the rigidbody on the same gameObject on the player locomotion, inputmanager script.
        inputManager = GetComponent<InputManager>();
        playerRigidBody = GetComponent<Rigidbody>();
        // will scan the scene for the thing that was tagged 'main camera'.
        cameraObject = Camera.main.transform;
        isGrounded = true; 
    }

    public void HandleAllMovement() {
        HandleRotation();
        // when you fall of cliff, you must be falling
        HandleFallingAndLanding();
        // when falling, you can't move
        if(playerManager.isInteracting) {
            return;
        }

        HandleMovement();
        //HandleRotation();//
    }
    private void HandleMovement() {
        // Up, Right = 1, Down, Left = -1.
        moveDirection = new Vector3(cameraObject.forward.x, 0f, cameraObject.forward.z) * inputManager.verticalInput;// the amount is = Movement input.
        // moveDirection = cameraObject.forward * inputManager.horizontalInput;
        // allows to move left and right based on horizontal input.
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize(); // keeps the circular direction of player to 1.
        moveDirection.y = 0;
        //If we are sprinting, select the sprinting speed
        //If we are running, select the running speed
        //If we are walking, slect the walking speed
        if (isSprinting) {
            moveDirection = moveDirection * sprintingSpeed;
        } else {
            if (inputManager.moveAmount >= 0.5f) {
                moveDirection = moveDirection * runningSpeed;
            } else {
                moveDirection = moveDirection * walkingSpeed;
            }
        }
        // moveDirection = moveDirection * runningSpeed;

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

    private void HandleFallingAndLanding() {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastOriginHeight;

        if (!isGrounded) {
            if (!playerManager.isInteracting) {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidBody.AddForce(transform.forward * leapingVelocity);
            playerRigidBody.AddForce(Vector3.down * fallingVelocity * inAirTimer);
        }
        // detects the ground layer
         if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistance, groundLayer)) {
            if (!isGrounded) { // missin !
                animatorManager.PlayTargetAnimation("Land", true);
            }

            inAirTimer = 0;
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }
}
