using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPortal : MonoBehaviour
{
   public GameObject leftPortal;
   public GameObject rightPortal;
   public GameObject mainCamera;
   public Transform laserSpawnPoint;
   public Color laserColor;

   private LineRenderer laser;

   void Start()
   {
      mainCamera = GameObject.FindWithTag("MainCamera");

      // Add a LineRenderer component to the laser spawn point object
      laser = laserSpawnPoint.gameObject.AddComponent<LineRenderer>();
      laser.startWidth = 0.1f;
      laser.endWidth = 0.1f;
      laser.material = new Material(Shader.Find("Sprites/Default"));

      // Set the initial color of the LineRenderer material
      laser.material.color = laserColor;
   }

   public void Throw_Portal(GameObject portal, Color laser_color)
   {
      // Update the laserColor variable to the given color value
      laserColor = laser_color;

      int x = Screen.width / 2;
      int y = Screen.height / 2;

      Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
      RaycastHit hit;

      // Create a layer mask that includes all layers except the "Player" layer
      LayerMask layerMask = LayerMask.GetMask("Player");
      layerMask = ~layerMask;

      if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
      {
         Quaternion hitObjectOrientation = Quaternion.LookRotation(hit.normal);
         portal.transform.position = hit.point;
         portal.transform.rotation = hitObjectOrientation;

         // Set the positions of the LineRenderer
         laser.SetPosition(0, laserSpawnPoint.position);
         laser.SetPosition(1, hit.point);

         // Update the color of the LineRenderer material
         laser.material.color = laserColor;

         // Enable the LineRenderer for half a second
         StartCoroutine(EnableLaser(0.25f));
      }
   }

   IEnumerator EnableLaser(float duration)
   {
      laser.enabled = true;
      yield return new WaitForSeconds(duration);
      laser.enabled = false;
   }
}
