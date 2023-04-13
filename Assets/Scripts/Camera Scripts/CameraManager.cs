using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    public Transform targetTransform; //the object the camera will follow
    public Transform cameraPivot; // The object the camera to pivot(look up and down)
    public Transform cameraTransform; // the transform of the actual camera object in the scene
    public LayerMask collisionLayers; // the layers we want our camera to collide with
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;
    
    public float cameraCollisionOffset = 0.2f; // how much the camera will jump off of objects its colliding with
    public float minimumCollisionOffSet = 0.2f;
    public float cameraCollisonRadius = 2;
    public float cameraFollowSpeed = 0.1f;
    public float cameraLookSpeed = 15;
    public float cameraPivotSpeed = 15;
    public float _camLookSmoothTime = 1;

    public float lookAngle; //Camera Looking Up and Down
    public float pivotAngle; //Camera looking left to right
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;



    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement() {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    private void FollowTarget() {
        // is going to update to the player's position from the current position that it
        //  in right now
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position + (transform.right), ref cameraFollowVelocity, cameraFollowSpeed);

            transform.position = targetPosition;
    }

    private void RotateCamera() {
        Vector3 rotation;
        Quaternion targetRotation;

        // lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        // pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        lookAngle = Mathf.Lerp(lookAngle, lookAngle + (inputManager.cameraInputX * cameraLookSpeed), _camLookSmoothTime * Time.deltaTime);
        pivotAngle = Mathf.Lerp(pivotAngle, pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed), _camLookSmoothTime * Time.deltaTime);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        // y transition
        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        // x transition
        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions() {
        float targetPosition = defaultPosition;

        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        // took off player as a layer to fix zooming in effect when walking towards the camera.

        if(Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisonRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers)) {
            
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            // the bigger the offset, the bigger the distance the camera is going to push away from the object
            targetPosition = -(distance - cameraCollisionOffset);;
        }

        if(Mathf.Abs(targetPosition) < minimumCollisionOffSet) {
            targetPosition = targetPosition - minimumCollisionOffSet;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
