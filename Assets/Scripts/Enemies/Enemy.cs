using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   [Header("Enemy")]
   public float SearchDistance = 5f;
   public float BaseContactDamage = 0.5f;
   public float BaseKnockbackStrength = 7.5f;
   public float BaseKnockbackStunDelay = 0.05f;
   public float WanderRadius = 5f;
   public float WanderCooldown = 1f;

   [Header("Gizmos")]
   public bool ShowSearchArea;
   public bool ShowWanderArea;
   public bool ShowPath;
   public bool ShowAhead;

   Health health;
   protected NavMeshAgent agent;
   private NavMeshPath path;
   protected Vector3 wanderHomePoint;
   private float wanderCooldownTimer;

   private void Awake()
   {
      health = gameObject.GetComponent<Health>();
      health.OnHit.AddListener(TakeDamage);

      agent = gameObject.GetComponent<NavMeshAgent>();
      agent.updateRotation = false;
      agent.updateUpAxis = false;
   }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   protected virtual void Start() { 
      wanderHomePoint = transform.position;
      path = new NavMeshPath();
   }

   // Update is called once per frame
   protected virtual void Update()
   {
      if (HasReachedDestination()) {
         if (wanderCooldownTimer == 0)
            wanderCooldownTimer = WanderCooldown;

         if (wanderCooldownTimer > 0)
            wanderCooldownTimer = Mathf.Clamp(wanderCooldownTimer - Time.deltaTime, 0, WanderCooldown);

         if (wanderCooldownTimer == 0)
         {
            var wanderDestination = wanderHomePoint + (Vector3)(Random.insideUnitCircle * WanderRadius);
            if (agent.CalculatePath(wanderDestination, path) && path.status == NavMeshPathStatus.PathComplete)
            {
               agent.SetPath(path);
            }
         }
      }
   }

   public void TakeDamage(Hit hit)
   {
      if (health.CurrentHealth == 0)
         Destroy(gameObject);
   }

   protected virtual void OnDrawGizmos()
   {
      if(ShowSearchArea)
      {
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(transform.position, SearchDistance);
      }

      if (ShowWanderArea)
      {
         Gizmos.color = Color.yellow;
         Gizmos.DrawWireSphere(wanderHomePoint, WanderRadius);
      }

      DrawPath(agent, ShowPath, ShowAhead);
   }

   private bool HasReachedDestination()
   {
      if(!agent.pathPending)
         if(agent.remainingDistance <= agent.stoppingDistance)
            if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
               return true;

      return false;
   }

   private static void DrawPath(NavMeshAgent agent, bool showPath, bool showAhead)
   {
      if (Application.isPlaying && agent != null)
      {
         if (showPath && agent.hasPath)
         {
            var corners = agent.path.corners;
            if (corners.Length < 2)
               return;

            for (int i = 0; i < corners.Length - 1; i++)
            {
               Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
               Gizmos.color = Color.blue;
               Gizmos.DrawSphere(agent.path.corners[i], 0.03f);
               Gizmos.color = Color.blue;
               Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
            }

            Debug.DrawLine(corners[0], corners[1], Color.blue);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(agent.path.corners[1], 0.03f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(agent.path.corners[0], agent.path.corners[1]);
         }

         if (showAhead)
         {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(agent.transform.position, agent.transform.up * 0.5f);
         }
      }
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
