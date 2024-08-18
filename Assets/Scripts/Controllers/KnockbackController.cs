using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackController : MonoBehaviour
{
   [SerializeField]
   private Rigidbody2D rb;

   private float delay;

   public UnityEvent OnBegin, OnDone;

   public void ApplyKnockback(Hit hit)
   {
      StopAllCoroutines();
      OnBegin?.Invoke();
      rb.linearVelocity = hit.KnockbackDirection * hit.KnockbackStrength;
      //rb.AddForce(, ForceMode2D.Impulse);
      delay = hit.KnockbackDelay;
      StartCoroutine(Reset());
   }

   private IEnumerator Reset()
   {
      yield return new WaitForSeconds(delay);
      rb.linearVelocity = Vector3.zero;
      delay = 0;
      OnDone?.Invoke();
   }
}
