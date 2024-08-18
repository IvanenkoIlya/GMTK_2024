using UnityEngine;

namespace Assets.Scripts
{
   public class HomingProjectile : Projectile
   {
      private Rigidbody2D rb;
      private GameObject target;

      protected override void Start()
      {
         base.Start();
         rb = GetComponent<Rigidbody2D>();
      }

      protected override void Update()
      {
         if(target != null)
         {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * ProjectileSpeed;
         }
      }
   }
}
