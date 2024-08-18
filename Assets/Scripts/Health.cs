using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
   [ReadOnly]
   public float CurrentHealth;
   [ReadOnly]
   public float MaxHealth;

   public UnityEvent<Hit> OnHit;
   public UnityEvent<float> OnHeal;

   [ReadOnly]
   public UnityEvent<float, float> OnHealthChanged;
   [ReadOnly]
   public UnityEvent<float, float> OnMaxHealthChanged;

   public void TakeDamage(Hit hit)
   {
      var prevHealth = CurrentHealth;
      CurrentHealth -= hit.Damage;
      ClampHealth();
      OnHit?.Invoke(hit);
      OnHealthChanged?.Invoke(prevHealth, CurrentHealth);
   }

   public void Heal(float healthGained)
   {
      var prevHealth = CurrentHealth;
      CurrentHealth += healthGained;
      ClampHealth();
      OnHeal?.Invoke(healthGained);
      OnHealthChanged?.Invoke(prevHealth, CurrentHealth);
   }

   public void AddMaxHealth(int amount, bool heal = true)
   {
      var prevMaxHealth = MaxHealth;
      MaxHealth += amount;
      OnMaxHealthChanged?.Invoke(prevMaxHealth, MaxHealth);
      if (heal)
         Heal(amount);
   }

   public void RemoveMaxHealth(int amount)
   {
      var prevMaxHealth = MaxHealth;
      MaxHealth -= amount;
      ClampHealth();
      OnMaxHealthChanged?.Invoke(prevMaxHealth, MaxHealth);
   }

   void ClampHealth() => CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
}

public struct Hit
{
   public float Damage;
   public float KnockbackStrength;
   public Vector2 KnockbackDirection;
   public float KnockbackDelay;

   public Hit(float damage)
   {
      Damage = damage;
      KnockbackStrength = 1f;
      KnockbackDirection = Vector2.zero;
      KnockbackDelay = 0f;
   }
}
