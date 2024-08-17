using UnityEngine;

public class Enemy : MonoBehaviour
{
   public GameObject projectile;
   public Transform bulletPos;
   public float shootTime = 2;
   public float maxHealth = 3;

   private float timer;
   private float health;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
      health = maxHealth;
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

   public void TakeDamage(float dmg)
   {
      health -= dmg;
      health = Mathf.Clamp(health, 0, maxHealth);
      if (health == 0)
      {
         Destroy(gameObject);
      }
   }

   void ShootAtPlayer()
   {
      if(GameObject.FindWithTag("Player") != null)
         Instantiate(projectile, bulletPos.position, Quaternion.identity);
   }
}
