using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
   public Transform player;
   public float relativeCameraPos = 0.002f;
   public Vector3 offset = new Vector3(0, 0, -10);

   // Update is called once per frame
   void Update()
   {
      var relativeMousePosition = player.position + (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
      var cameraPos = Vector2.Lerp(player.position, relativeMousePosition, relativeCameraPos);
      transform.position = new Vector3(cameraPos.x, cameraPos.y, 0) + offset;
   }
}
