using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPortal : MonoBehaviour
{
   public GameObject leftPortal;
   public GameObject rightPortal;
   public GameObject mainCamera;
   // Start is called before the first frame update
   void Start()
   {
      mainCamera = GameObject.FindWithTag("MainCamera");
   }

   // Update is called once per frame
   // void Update()
   // {
   //    if (Input.GetMouseButtonDown(0))
   //    {
   //       Debug.Log("right click");
   //       Throw_Portal(leftPortal);
   //    }
   //    if (Input.GetMouseButtonDown(1))
   //    {
   //       Debug.Log("left click");
   //       Throw_Portal(rightPortal);
   //    }
   // }

   public void Throw_Portal(GameObject portal)
   {
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
      }
   }
}
