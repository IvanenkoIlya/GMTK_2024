using UnityEngine;

public class Enemy : MonoBehaviour
{
   public float maxHealth = 3;

   private float health;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
      health = maxHealth;
   }

   // Update is called once per frame
   void Update()
   {
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

}
