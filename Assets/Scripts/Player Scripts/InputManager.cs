using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput; // a directon to Left/Right and Up/Down.
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool b_Input;
    public bool jump_input;

    private void Awake() {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
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

            // when the b/shift iput is hit, turns b_input to true
            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;

            playerControls.PlayerActions.Jump.performed += i => jump_input = true;
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
        HandleSprintingInput();
        HandleJumpingInput();
        //HandleActionInput
    }

    private void HandleMovementInput() {
        verticalInput = movementInput.y; // giving it the value of y axis
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        //math.clamp01 clamps value to 0 or 1
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValue(0, moveAmount, playerLocomotion.isSprinting);
    }
    private void HandleSprintingInput() {
        if (b_Input && moveAmount > 0.5f) {
            playerLocomotion.isSprinting = true;
        } else {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput() {
        if(jump_input) {
            jump_input = false;
            playerLocomotion.HandleJumping();
        }
    }
}
