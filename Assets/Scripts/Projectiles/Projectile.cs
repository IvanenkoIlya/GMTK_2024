using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   public GameObject collisionParticle;
   public Vector2 Direction;
   public float ProjectileSpeed = 1f;
   public float Damage = 0.5f;
   public List<string> IgnoreTags = new List<string> { "Projectile" };

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   protected virtual void Start()
   {
      GetComponent<Rigidbody2D>().linearVelocity = Direction * ProjectileSpeed;
   }

   // Update is called once per frame
   protected virtual void Update()
   {

   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (IgnoreTags.Contains(collision.gameObject.tag))
         return;
      collision.gameObject.GetComponent<Health>()?.TakeDamage(new Hit(Damage));
      Instantiate(collisionParticle, transform.position, Quaternion.identity);
      Destroy(gameObject);
   }
}
