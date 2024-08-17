using UnityEngine;

public class Projectile : MonoBehaviour
{
   protected GameObject player;
   protected Rigidbody2D rb;

   public float movementSpeed = 1;
   public int damage = 1;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      player = GameObject.FindWithTag("Player");

      Vector3 direction = player.transform.position - transform.position;
      rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * movementSpeed;
   }

   // Update is called once per frame
   void Update()
   {

   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.CompareTag("Player"))
      {
         collision.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
      }
      if (!collision.gameObject.CompareTag("Projectile") & !collision.gameObject.CompareTag("Enemy")) {
         Destroy(gameObject);
      }
   }
}
