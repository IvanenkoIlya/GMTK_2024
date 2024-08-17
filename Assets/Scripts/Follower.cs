using UnityEngine;

public class Follower : MonoBehaviour
{
   public GameObject Target;
   public float MoveSpeed = 5f;
   public float FollowDistance = 3f;
   public float ShootingRange = 10f;

   private float timer;
   public GameObject projectile;
   public Transform bulletPos;
   public float shootTime = 2;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      if(Target != null && Vector3.Distance(transform.position, Target.transform.position) > FollowDistance)
         transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, MoveSpeed * Time.deltaTime);

      timer += Time.deltaTime;

      if (timer > shootTime)
      {
         timer = 0;
         ShootAtEnemy();
      }
   }

   void ShootAtEnemy()
   {
      var enemyList = Physics2D.OverlapCircleAll(transform.position, ShootingRange);
      var dist = Mathf.Infinity;
      GameObject enemyTarget = null;
      foreach (var enemyCollider in enemyList)
      {
         var enemy = enemyCollider.gameObject;
         var hit = Physics2D.Raycast(transform.position, enemy.transform.position - transform.position);
         if (hit.collider != null && hit.collider.gameObject.tag == "Enemy")
         {
            var newDist = Vector2.Distance(enemyCollider.transform.position, transform.position);
            if (newDist < dist)
            {
               dist = newDist;
               enemyTarget = enemy;
            }
         }
      }
      if (enemyTarget != null)
      {
         var temp = Instantiate(projectile, bulletPos.position, Quaternion.identity);
         temp.GetComponent<FriendlyProjectile>().target = enemyTarget;
      }
   }
}
