using UnityEngine;

public class ChargingEnemy : Enemy
{
   public float ChargeSpeed = 7f;
   public float ChargeUpTime = 1.3f;
   public float ChargeCooldown = 1f;
   public float ChargeContactDamage = 1.5f;
   public float ChargingKnockbackStrength = 1f;
   public float ChargineKnockbackStunDelay = 0.15f;

   private EnemyState state;
   private float lockOnTimer;
   private float rechargeTimer;
   private Vector3 chargePosition;
   private Vector3 chargeDirection;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {

   }

   // Update is called once per frame
   protected override void Update()
   {
      var player = GameObject.FindWithTag("Player");
      RaycastHit2D hit;

      switch (state)
      {
         case EnemyState.Idle:
            base.Update();

            // Look for player within range
            if (player != null && (player.transform.position - transform.position).magnitude < SearchDistance)
            {
               hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);

               if (hit.collider != null && hit.collider.gameObject.tag == "Player")
               {
                  // TODO play a little animation/particle here
                  state = EnemyState.LockedOn;
                  lockOnTimer = 0;
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
               lockOnTimer += Time.deltaTime;
               if (lockOnTimer > ChargeUpTime)
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
            transform.position = Vector3.MoveTowards(transform.position, chargePosition, ChargeSpeed * Time.deltaTime);
            if (Mathf.Abs((chargePosition - transform.position).magnitude) < 0.001f)
            {
               // TODO: create AOE?
               state = EnemyState.Recharging;
               rechargeTimer = ChargeCooldown;
            }
            break;
         case EnemyState.Recharging:
            // Wait until cooldown
            rechargeTimer = Mathf.Clamp(rechargeTimer - Time.deltaTime, 0, ChargeCooldown);
            if (rechargeTimer <= 0)
               state = EnemyState.Idle;
            break;
      }
   }

   protected override void OnDrawGizmos()
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

      base.OnDrawGizmos();
   }

   protected override void OnCollisionEnter2D(Collision2D collision)
   {
      if (collision.gameObject.CompareTag("Player"))
      {
         if (state == EnemyState.Charging)
         {
            collision.gameObject.GetComponent<Health>().TakeDamage(new Hit
            {
               Damage = ChargeContactDamage,
               KnockbackDirection = chargeDirection.normalized,
               KnockbackStrength = ChargingKnockbackStrength,
               KnockbackDelay = ChargineKnockbackStunDelay
            });
         }
         else
         {
            base.OnCollisionEnter2D(collision);
         }
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
