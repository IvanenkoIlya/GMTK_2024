using UnityEngine;

namespace Assets.Scripts
{
   public class HomingProjectile : Projectile
   {
      private void Update()
      {
         Vector3 direction = player.transform.position - transform.position;
         rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * movementSpeed;
      }
   }
}
