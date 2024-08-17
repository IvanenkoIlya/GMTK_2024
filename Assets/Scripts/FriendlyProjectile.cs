using UnityEngine;

public class FriendlyProjectile : MonoBehaviour
{
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
         //Implement enemy taking damage later
         //collision.gameObject.GetComponent<EnemyStats>().TakeDamage(damage); 
         print("Hit!");
      }

      Destroy(gameObject);
   }
}
