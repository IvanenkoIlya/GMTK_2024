using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
   [Header("Camera Settings")]
   public List<GameObject> FocusObjects;
   public Vector3 CameraOffset = new Vector3(0, 0, -10);
   public float CameraMoveSpeed = 5f;
   public float CameraZoomSpeed = 3f;
   public float MinZoom = 5f, MaxZoom = 15f;
   public float Padding = 5f;

   private Vector2 maxDistanceFromCenter;

   [Header("Debug")]
   public GameObject CenterPoint;
   public float debugWidth = 800, debugHeight = 1600;
   private string debugString = string.Empty;

   private Vector3 targetCameraCenter;
   private float targetCameraZoom;

   private void Start()
   {
      UpdateMaxDistanceFromCenter();
   }

   // Update is called once per frame
   void Update()
   {
      debugString = string.Empty;
      var screenAspectRatio = (float)Screen.width / (float)Screen.height;

      UpdateMaxDistanceFromCenter();

      var focusedObjects = new Dictionary<GameObject, Vector3>();
      CalculateCameraCenter(out targetCameraCenter, out focusedObjects);
      targetCameraCenter += CameraOffset;

      var furthestVertically = focusedObjects.Values.Max(x => Mathf.Abs(x.y));
      var furthestHorizontally = focusedObjects.Values.Max(x => Mathf.Abs(x.x));
      var verticalBounds = furthestVertically + Padding;
      var horizontalBounds = (furthestHorizontally + Padding) / screenAspectRatio;
      targetCameraZoom = Mathf.Clamp(Mathf.Max(verticalBounds, horizontalBounds), MinZoom, MaxZoom);

      CenterPoint.transform.position = targetCameraCenter;

      Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetCameraCenter, CameraMoveSpeed * Time.deltaTime);
      Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetCameraZoom, CameraZoomSpeed * Time.deltaTime);

      // Debug Info
      debugString += $"aspectRatio: {screenAspectRatio}\n";
      debugString += $"Current Camera position: {Camera.main.transform.position}\n";
      debugString += $"Target Camera position: {targetCameraCenter}\n";
      debugString += $"Current Camera zoom: {Camera.main.orthographicSize}\n";
      debugString += $"Target Camera zoom: {targetCameraZoom}\n";
      debugString += "-------------------------------------------\n";
      foreach (var obj in focusedObjects)
         debugString += $"\t{obj}\n";
      debugString += $"furthestVertically: {furthestVertically}\n";
      debugString += $"furthestHorizontally: {furthestHorizontally}\n";
      debugString += $"verticalBounds: {verticalBounds}\n";
      debugString += $"horizontalBounds: {horizontalBounds}\n";
   }

   private void UpdateMaxDistanceFromCenter()
   {
      maxDistanceFromCenter = new Vector2(MaxZoom * ((float)Screen.width / (float)Screen.height) - Padding, MaxZoom - Padding);
   }

   private void CalculateCameraCenter(out Vector3 center, out Dictionary<GameObject, Vector3> focusedObjects)
   {
      int size = 0;

      center = Vector3.zero;
      focusedObjects = new Dictionary<GameObject, Vector3>();

      foreach(var obj in FocusObjects)
      {
         var newCenter = center + (obj.transform.position - center) / ++size;

         // If next obj is too far from new calculated center,
         // return old center and all previously included objects
         var distanceFromCenter = obj.transform.position - newCenter;
         if (Mathf.Abs(distanceFromCenter.x) >= maxDistanceFromCenter.x || Mathf.Abs(distanceFromCenter.y) >= maxDistanceFromCenter.y)
            return;

         // If any of the previous objects are too far from new center,
         // return old center and all previously included objects
         foreach (var focusedObject in focusedObjects)
         {
            var newDistance = focusedObject.Value - newCenter;
            if (Mathf.Abs(newDistance.x) >= maxDistanceFromCenter.x || Mathf.Abs(newDistance.y) >= maxDistanceFromCenter.y)
               return;
         }

         center = newCenter;
         List<GameObject> keys = new List<GameObject>(focusedObjects.Keys);
         foreach(var focusedObject in keys)
            focusedObjects[focusedObject] = focusedObject.transform.position - center;
         focusedObjects.Add(obj, distanceFromCenter);
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.blue;
      Gizmos.DrawCube(targetCameraCenter, new Vector3(0.2f, 0.2f, 0.2f));
   }

   private void OnGUI()
   {
      GUI.color = Color.black;
      GUI.Label(new Rect(0, 0, debugWidth, debugHeight), debugString);
   }

   [Serializable]
   public struct FollowObject
   {
      public int Priority;
      public GameObject Object;
   }
}
