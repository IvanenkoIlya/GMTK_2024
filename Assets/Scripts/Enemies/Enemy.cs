using UnityEngine;

public class Enemy : MonoBehaviour
{
   Health health;

   private void Awake()
   {
      health = gameObject.GetComponent<Health>();
      health.OnHit.AddListener(TakeDamage);
   }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
      
   }

   // Update is called once per frame
   void Update()
   {
   }

   public void TakeDamage(Hit hit)
   {
      if(health.CurrentHealth == 0)
         Destroy(gameObject);
   }
}
