using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
   public float SearchDistance = 5f;
   public float ChargeSpeed = 7f;
   public float ChargeDelay = 1.3f;
   public float ChargeCooldown = 1f;
   public float ContactDamage = 1f;
   public float KnockbackStrength = 1f;
   public float KnockbackStunDelay = 0.15f;

   private EnemyState state;
   private float lockTimer;
   private float rechargeTimer;
   private Vector3 chargePosition;
   private Vector3 chargeDirection;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      var player = GameObject.FindWithTag("Player");
      RaycastHit2D hit;

      switch (state)
      {
         case EnemyState.Idle:
            // Look for player within range
            if (player != null && (player.transform.position - transform.position).magnitude < SearchDistance)
            {
               hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
               
               if (hit.collider != null && hit.collider.gameObject.tag == "Player")
               {
                  // TODO play a little animation/particle here
                  state = EnemyState.LockedOn;
                  lockTimer = 0;
               }
            }
            break;
         case EnemyState.LockedOn:
            // Remain locked on until Charge delay
            hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);

            if (player != null &&
               (player.transform.position - transform.position).magnitude < SearchDistance &&
               hit.collider != null &&
               hit.collider.gameObject.tag == "Player")
            {
               // Charge up
               lockTimer += Time.deltaTime;
               if (lockTimer > ChargeDelay)
               {
                  state = EnemyState.Charging;
                  chargePosition = player.transform.position;
                  chargeDirection = chargePosition - transform.position;
               }
            }
            else
            {
               state = EnemyState.Idle;
            }
            break;
         case EnemyState.Charging:
            // Move towards player position
            print("Time to charge!");
            transform.position = Vector3.MoveTowards(transform.position, chargePosition, ChargeSpeed * Time.deltaTime);
            if (Mathf.Abs((chargePosition - transform.position).magnitude) < 0.001f)
            {
               // TODO create AOE?
               state = EnemyState.Recharging;
               rechargeTimer = 0;
            }
            break;
         case EnemyState.Recharging:
            print("Recharging...");
            // Wait until cooldown
            rechargeTimer += Time.deltaTime;
            if (rechargeTimer > ChargeCooldown)
            {
               state = EnemyState.Idle;
            }
            break;
      }
   }

   private void OnDrawGizmos()
   {
      var player = GameObject.FindWithTag("Player");

      if (state == EnemyState.LockedOn && player != null)
      {
         if ((player.transform.position - transform.position).magnitude < SearchDistance)
            Gizmos.color = Color.red;
         else
            Gizmos.color = Color.yellow;
         Gizmos.DrawLine(transform.position, player.transform.position);
      }

      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, SearchDistance);
   }

   private void OnCollisionEnter2D(Collision2D collision)
   {
      if (collision.gameObject.CompareTag("Player"))
      {
         collision.gameObject.GetComponent<Health>().TakeDamage(new Hit
         {
            Damage = ContactDamage,
            KnockbackDirection = chargeDirection.normalized,
            KnockbackStrength = KnockbackStrength,
            KnockbackDelay = KnockbackStunDelay
         });
      }
   }

   private enum EnemyState
   {
      Idle,
      LockedOn,
      Charging,
      Recharging
   }
}
