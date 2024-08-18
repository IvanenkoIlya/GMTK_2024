using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
   public GameObject Target;
   public float MoveSpeed = 5f;
   public float FollowDistance = 3f;
   public float ShootTime = 2;
   public float ShootingRange = 10f;

   public GameObject projectile;
   public Transform bulletPos;

   private float shootCooldownTimer;
   private NavMeshAgent agent;

   private void Awake()
   {
      agent = GetComponent<NavMeshAgent>();
      agent.updateRotation = false;
      agent.updateUpAxis = false;
   }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start() { }

   // Update is called once per frame
   void Update()
   {
      if (Target != null)
         agent.destination = Target.transform.position;

      shootCooldownTimer = Mathf.Clamp(shootCooldownTimer - Time.deltaTime, 0, ShootTime);

      if(shootCooldownTimer <= 0)
         ShootAtEnemy();
   }

   void ShootAtEnemy()
   {
      var enemyList = Physics2D.OverlapCircleAll(transform.position, ShootingRange).Select(x => x.gameObject).ToList();
      enemyList.OrderBy(x => x.transform.position - transform.position);
      foreach(var enemy in enemyList)
      {
         var hit = Physics2D.Raycast(bulletPos.position, enemy.transform.position - bulletPos.position);
         if(hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
         {
            var temp = Instantiate(projectile, bulletPos.position, Quaternion.identity);
            temp.GetComponent<FriendlyProjectile>().target = enemy;
            shootCooldownTimer = ShootTime;
            break;
         }
      }
   }

   private void OnDrawGizmos()
   {
      if(Application.isPlaying && agent != null)
      {
         Gizmos.color = Color.green;
         Gizmos.DrawSphere(agent.destination, 0.1f);
         Gizmos.DrawRay(transform.position, agent.destination - transform.position);
      }
   }
}
