using NUnit.Framework;
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
         print("full heart");
         //Add a full heart to the bar
      }

      if (health % 2 ==1)
      {
         print("half heart");
         //Add half a heart to the bar
      }
      //Instantiate the whole bar as a game object?
   }
}
