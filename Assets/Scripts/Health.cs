using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
   [ReadOnly]
   public float CurrentHealth;
   [ReadOnly]
   public float MaxHealth;
   [ReadOnly]
   public float MaxTotalHealth;

   public UnityEvent<Hit> OnHit;
   public UnityEvent<float> OnHeal;

   public void TakeDamage(Hit hit)
   {
      CurrentHealth -= hit.Damage;
      ClampHealth();
      OnHit?.Invoke(hit);
   }

   public void Heal(float healthGained)
   {
      CurrentHealth += healthGained;
      ClampHealth();
      OnHeal?.Invoke(healthGained);
   }

   public void AddHealth()
   {
      if (MaxHealth < MaxTotalHealth) {
         MaxHealth += 1;
         Heal(1);
      }
   }

   void ClampHealth() => CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
}

public struct Hit
{
   public float Damage;
   public float KnockbackStrength;
   public Vector2 KnockbackDirection;
   public float KnockbackDelay;
}
