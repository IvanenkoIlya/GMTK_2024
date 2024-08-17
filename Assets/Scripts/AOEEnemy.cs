using UnityEngine;

public class AOEEnemy : MonoBehaviour
{
   public float SearchDistance = 5f;
   public float ChargeSpeed = 7f;
   public float ChargeDelay = 1.3f;
   public float ChargeCooldown = 1f;

   private EnemyState state;
   private float lockTimer;
   private float rechargeTimer;
   private Vector3 chargePosition;


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
               print("Casting ray!");

               if (hit.collider != null && hit.collider.gameObject.tag == "Player")
               {
                  print("Gotcha! Player hit, locking on.");
                  // TODO play a little animation/particle here
                  state = EnemyState.LockedOn;
                  lockTimer = 0;
               }
            }
            break;
         case EnemyState.LockedOn:
            // Remain locked on until Charge delay
            hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
            print("Lockon ray casting...");
            if (player != null &&
               (player.transform.position - transform.position).magnitude < SearchDistance &&
               hit.collider != null &&
               hit.collider.gameObject.tag == "Player")
            {
               // Charge up
               lockTimer += Time.deltaTime;
               print("Player hit, charging up! lockTimer is " + lockTimer);
               if (lockTimer > ChargeDelay)
               {
                  state = EnemyState.Charging;
                  chargePosition = player.transform.position;
               }
            }
            else
            {
               print("Player not found, idling...");
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

   private enum EnemyState
   {
      Idle,
      LockedOn,
      Charging,
      Recharging
   }
}
