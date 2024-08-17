using UnityEngine;

public class Enemy : MonoBehaviour
{
   public GameObject projectile;
   public Transform bulletPos;
   public float shootTime = 2;

   private float timer;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      timer += Time.deltaTime;

      if (timer > shootTime)
      {
         timer = 0;
         ShootAtPlayer();
      }
   }

   void ShootAtPlayer()
   {
      if(GameObject.FindWithTag("Player") != null)
         Instantiate(projectile, bulletPos.position, Quaternion.identity);
   }
}
