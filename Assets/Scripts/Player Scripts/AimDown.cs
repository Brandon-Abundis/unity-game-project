using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDown : MonoBehaviour
{
    InputManager inputManager;

    public Camera camera;
    public bool isAimedDown;

    private void Awake() {
        // placing the rigidbody on the same gameObject on the player locomotion, inputmanager script.
        inputManager = GetComponent<InputManager>();
        
    }

    private void HandleAimDown() {
        if (isAimedDown == true) {
            camera.fieldOfView = 60 / 2;
            Debug.Log(isAimedDown);
        } else {
            camera.fieldOfView = 60;
        }
        
    }
}
