using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepThroughPortal : MonoBehaviour
{
   public Transform teleportTarget;
   public GameObject otherPortal;

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         // Calculate the player's position relative to this portal's transform
         Vector3 localPlayerPosition = transform.InverseTransformPoint(other.transform.position);

         // Set the player position to the teleport target position relative to the other portal's transform
         other.transform.position = otherPortal.transform.TransformPoint(teleportTarget.transform.localPosition + localPlayerPosition);

         // Rotate the player to face away from the other portal
         Vector3 playerForward = other.transform.forward;
         Vector3 cameraForward = Camera.main.transform.forward;
         float angle = Vector3.SignedAngle(playerForward, cameraForward, Vector3.up);
         Vector3 portalForward = otherPortal.transform.forward;
         Vector3 playerUp = other.transform.up;
         Vector3 portalUp = otherPortal.transform.up;
         Vector3 forwardRotationAxis = Vector3.Cross(playerForward, portalForward);
         Vector3 upRotationAxis = Vector3.Cross(playerUp, portalUp);
         portalForward = Quaternion.AngleAxis(angle, Vector3.up) * portalForward;
         other.transform.rotation = Quaternion.AngleAxis(0f, upRotationAxis) * Quaternion.LookRotation(portalForward, upRotationAxis);
      }
   }





}
