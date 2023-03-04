using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager; // calling the input manager scripts
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;

    private void Awake() {
        // scripts will be resting on the same game object
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update() {
        inputManager.HandleAllInputs();
    }

    // the reason we use fixed update is bec, when working with a rigid body,
    //  it behaves much nicer and all things that handle moving it should be
    //  handled under fixed update
    private void FixedUpdate() {
        playerLocomotion.HandleAllMovement();
    }

    //calls after the frame has ended
    private void LateUpdate() {
        cameraManager.HandleAllCameraMovement();
    }
}
