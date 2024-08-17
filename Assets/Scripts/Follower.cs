using UnityEngine;

public class Follower : MonoBehaviour
{
   public GameObject Target;
   public float MoveSpeed = 5f;
   public float FollowDistance = 3f;

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
      if (GameObject.FindWithTag("Enemy") != null)
         Instantiate(projectile, bulletPos.position, Quaternion.identity);
   }
}
