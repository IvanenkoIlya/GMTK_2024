using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   private int currentHealth;
   private Rigidbody2D rb;
   private Vector2 inputVector;

   public float movementSpeed = 5f;
   public float SprintModifier = 1.5f;


   InputAction moveAction;
   InputAction sprintAction;
   InputAction rollAction;
   InputAction attackAction;

   private Animator characterAnimator;
   private bool rolling;

   void Awake()
   {
      rb = GetComponent<Rigidbody2D>();
    }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
      moveAction = InputSystem.actions.FindAction("Move");
      sprintAction = InputSystem.actions.FindAction("Sprint");
      rollAction = InputSystem.actions.FindAction("Roll");
      attackAction = InputSystem.actions.FindAction("Attack");
      characterAnimator = GetComponentInChildren<Animator>();
   }

   // Update is called once per frame
   void Update()
   {
      inputVector = moveAction.ReadValue<Vector2>().normalized;
      rb.linearVelocity = inputVector * movementSpeed * (sprintAction.IsPressed() ? SprintModifier : 1f);
   }
}
