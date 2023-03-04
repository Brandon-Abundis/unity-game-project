using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;

    int horizontal;
    int vertical;

    private void Awake() {
        animator = GetComponent<Animator>();
        // referencing values from animator
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting) {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorValue(float horizontalMovement, float verticalMovement, bool isSprinting) {
        // Animation Snapping
        // snaps to a walk or a run, rounds the values, makes it cleaner
        float snappedHorizontal;
        float snappedVertical;

        // if input from controller and its >0 and <0.55, it will always be 0.5
        // will not make the walking and running not blend
        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f) {
            snappedHorizontal = 0.5f;
        } else if (horizontalMovement > 0.55f) {
            snappedHorizontal = 1;
        } else if (horizontalMovement < 0 && horizontalMovement > -0.55f) {
            snappedHorizontal = -0.5f;
        } else if (horizontalMovement < -0.55f) {
            snappedHorizontal = -1;
        } else {
            snappedHorizontal = 0;
        }
        #endregion

        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f) {
            snappedVertical = 0.5f;
        } else if (verticalMovement > 0.55f) {
            snappedVertical = 1;
        } else if (verticalMovement < 0 && verticalMovement > -0.55f) {
            snappedVertical = -0.5f;
        } else if (verticalMovement < -0.55f) {
            snappedVertical = -1;
        } else {
            snappedVertical = 0;
        }
        #endregion

        if (isSprinting) {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }
        // -> "SetFloat(int id, float value, float dampTime, float deltaTime)"
        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }
}
