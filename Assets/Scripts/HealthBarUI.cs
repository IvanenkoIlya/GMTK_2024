using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
   public Sprite HalfHealth;
   public Sprite FullHealth;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {

   }

   public void SetHealth(int health)
   {
      print(health);
      for (int i = 0; i < health/2; i++ )
      {
         Instantiate(FullHealth, transform.position + new Vector3(i*FullHealth.border.x, 0, 0), Quaternion.identity);
      }

      if (health % 2 ==1)
      {
         Instantiate(HalfHealth, transform.position + new Vector3(health / 2 * FullHealth.border.x, 0, 0), Quaternion.identity);
      }
   }
}
