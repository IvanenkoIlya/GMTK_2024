using UnityEngine;

public class FriendlyProjectile : MonoBehaviour
{
   public GameObject collisionParticle;

   protected GameObject enemy;
   protected Rigidbody2D rb;

   public float movementSpeed = 1;
   public int damage = 1;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      enemy = GameObject.FindWithTag("Enemy");

      Vector3 direction = enemy.transform.position - transform.position;
      rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * movementSpeed;
   }

   // Update is called once per frame
   void Update()
   {

   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.CompareTag("Enemy"))
      {
         collision.gameObject.GetComponent<Health>().TakeDamage(new Hit(damage));
      }
      if (!collision.gameObject.CompareTag("Projectile") & !collision.gameObject.CompareTag("Player")) {
         Instantiate(collisionParticle, this.transform.position, Quaternion.identity);
         Destroy(gameObject);
      }
   }
}
