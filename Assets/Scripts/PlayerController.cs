using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private int currentHealth;
   private Rigidbody2D rb;
   private Vector2 inputVector;

   public Vector2 movementSpeed = new Vector2(5f,5f);

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Awake()
   {
      rb = GetComponent<Rigidbody2D>();
    }

   // Update is called once per frame
   void Update()
   {
      inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
      rb.linearVelocity = inputVector * movementSpeed;
   }
}
