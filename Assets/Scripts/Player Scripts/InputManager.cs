using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    PlayerControls playerControls;
    AnimatorManager animatorManager;

    public Vector2 movementInput; // a directon to Left/Right and Up/Down.
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    private void Awake() {
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void OnEnable() {
        // meaning the variable is not filled with anything.
        if(playerControls == null) {
            // set up that varaible as a new instance of the control.
            playerControls = new PlayerControls();
            // every input on keyboard/joystick is going to be recorded to the 
            //  vector 2 movementInput variable.
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }
    private void OnDisable() {
        // if the gameObject that the scrip is on id disabled, then the player controls
        //  will be set to disabled.
        playerControls.Disable();
    }

    public void HandleAllInputs() {
        // incapsulating code into one function
        HandleMovementInput();
        //HandleJumpInput
        //HandleActionInput
    }

    private void HandleMovementInput() {
        verticalInput = movementInput.y; // giving it the value of y axis
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        //math.clamp01 clamps value to 0 or 1
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValue(0, moveAmount);
    }
}
