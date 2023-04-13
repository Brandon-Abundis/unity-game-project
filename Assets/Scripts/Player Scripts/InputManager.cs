using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
   PlayerControls playerControls;
   PlayerLocomotion playerLocomotion;
   AnimatorManager animatorManager;

   ThrowPortal throwPortal;

   public Vector2 movementInput; // a directon to Left/Right and Up/Down.
   public Vector2 cameraInput;

   public float cameraInputX;
   public float cameraInputY;

   public float moveAmount;
   public float verticalInput;
   public float horizontalInput;

   public bool b_Input;
   public bool jump_input;

   [Header("portals")]
   public bool right_trigger;
   public bool left_trigger;

   public GameObject leftPortal;
   public GameObject rightPortal;
   public GameObject camera_obj;

   private void Awake()
   {
      animatorManager = GetComponent<AnimatorManager>();
      playerLocomotion = GetComponent<PlayerLocomotion>();
      throwPortal = camera_obj.GetComponent<ThrowPortal>();
   }

   private void OnEnable()
   {
      // meaning the variable is not filled with anything.
      if (playerControls == null)
      {
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

         playerControls.PlayerActions.RightTigger.performed += i => right_trigger = true;
         playerControls.PlayerActions.LeftTrigger.performed += i => left_trigger = true;
      }

      playerControls.Enable();
   }
   private void OnDisable()
   {
      // if the gameObject that the scrip is on id disabled, then the player controls
      //  will be set to disabled.
      playerControls.Disable();
   }

   public void HandleAllInputs()
   {
      // incapsulating code into one function
      HandleMovementInput();
      HandleSprintingInput();
      HandleJumpingInput();
      HandleThrowPortals();
      //HandleActionInput
   }

   private void HandleMovementInput()
   {
      verticalInput = movementInput.y; // giving it the value of y axis
      horizontalInput = movementInput.x;

      cameraInputY = cameraInput.y;
      cameraInputX = cameraInput.x;

      //math.clamp01 clamps value to 0 or 1
      moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
      animatorManager.UpdateAnimatorValue(0, moveAmount, playerLocomotion.isSprinting);
   }
   private void HandleSprintingInput()
   {
      if (b_Input && moveAmount > 0.5f)
      {
         playerLocomotion.isSprinting = true;
      }
      else
      {
         playerLocomotion.isSprinting = false;
      }
   }

   private void HandleJumpingInput()
   {
      if (jump_input)
      {
         jump_input = false;
         playerLocomotion.HandleJumping();
      }
   }

   private void HandleThrowPortals()
   {
      if (right_trigger)
      {
         Debug.Log("left click");
         right_trigger = false;
         throwPortal.Throw_Portal(rightPortal);
      }
      if (left_trigger)
      {
         Debug.Log("right click");
         left_trigger = false;
         throwPortal.Throw_Portal(leftPortal);
      }
   }
}
