using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    PlayerControls playerControls;

    public Vector2 movementInput; // a directon to Left/Right and Up/Down.
    public float verticalInput;
    public float horizontalInput;

    private void OnEnable() {
        // meaning the variable is not filled with anything.
        if(playerControls == null) {
            // set up that varaible as a new instance of the control.
            playerControls = new PlayerControls();
            // every input on keyboard/joystick is going to be recorded to the 
            //  vector 2 movementInput variable.
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }
    private void OnDisable() {
        // if the gameObject that the scrip is on id disabled, then the player controls
        //  will be set to disabled.
        playerControls.Disable();
    }

    private void HandleMovementInput() {
        verticalInput = movementInput.y; // giving it the value of y axis
        horizontalInput = movementInput.x;
    }
}
