using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
   public GameObject Actor;
   public GameObject HeartContainerPrefab;
   public List<Sprite> HealthSprites;

   private Health actorHealth;
   private List<GameObject> heartContainers = new List<GameObject>();

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
      if(Actor != null)
      {
         actorHealth = Actor.GetComponent<Health>();
         actorHealth.OnHealthChanged.AddListener(UpdateHealthHUD);
         actorHealth.OnMaxHealthChanged.AddListener(UpdateMaxHealthHUD);
      }

      UpdateMaxHealthHUD(0f, actorHealth.MaxHealth);
      UpdateHealthHUD(0f, actorHealth.CurrentHealth);
   }

   public void UpdateHealthHUD(float previousHealth, float newHealth)
   {
      foreach (GameObject container in heartContainers)
      {
         int healthSpriteIndex;

         if (newHealth >= 1)
            healthSpriteIndex = HealthSprites.Count - 1;
         else if (newHealth <= 0)
            healthSpriteIndex = 0;
         else
            healthSpriteIndex = (int)Mathf.Floor(newHealth * HealthSprites.Count);

         container.GetComponent<Image>().sprite = HealthSprites[healthSpriteIndex];
         newHealth -= 1;
      }
   }

   public void UpdateMaxHealthHUD(float previousMaxHealth, float newMaxHealth)
   {
      if (previousMaxHealth < newMaxHealth)
         AddHeartContainers((int)Mathf.Ceil(newMaxHealth - previousMaxHealth));
      else if(previousMaxHealth > newMaxHealth)
         RemoveHeartContainers((int)Mathf.Floor(previousMaxHealth - newMaxHealth));
   }

   void AddHeartContainers(int count)
   {
      for (int i = 0; i < count; i++)
      {
         var temp = Instantiate(HeartContainerPrefab);
         temp.transform.SetParent(gameObject.transform, false);
         heartContainers.Add(temp);
      }
   }

   void RemoveHeartContainers(int count)
   {
      for(int i = 0; i < count; i++)
      {
         var temp = heartContainers.Last();
         heartContainers.RemoveAt(heartContainers.Count-1);
         Destroy(temp);
      }
   }
}
