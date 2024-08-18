using UnityEngine;

public class Enemy : MonoBehaviour
{
   public float SearchDistance = 5f;
   public float BaseContactDamage = 0.5f;
   public float BaseKnockbackStrength = 3f;
   public float BaseKnockbackStunDelay = 0.05f;

   Health health;

   private void Awake()
   {
      health = gameObject.GetComponent<Health>();
      health.OnHit.AddListener(TakeDamage);
   }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {

   }

   // Update is called once per frame
   protected virtual void Update()
   {
      // TODO: add wandering here
   }

   public void TakeDamage(Hit hit)
   {
      if (health.CurrentHealth == 0)
         Destroy(gameObject);
   }

   protected virtual void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, SearchDistance);
   }

   protected virtual void OnCollisionEnter2D(Collision2D collision)
   {
      if (collision.gameObject.CompareTag("Player"))
      {
         collision.gameObject.GetComponent<Health>().TakeDamage(new Hit
         {
            Damage = BaseContactDamage,
            KnockbackDirection = (collision.gameObject.transform.position - gameObject.transform.position).normalized,
            KnockbackStrength = BaseKnockbackStrength,
            KnockbackDelay = BaseKnockbackStunDelay
         });
      }
   }
}
