using UnityEngine;

public class ShootingEnemy : Enemy
{
   public GameObject Projectile;
   public Transform ProjectileOrigin;
   public float ShootCooldown = 2f;

   private float shootCooldownTimer;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {

   }

   // Update is called once per frame
   protected override void Update()
   {
      base.Update();

      if(shootCooldownTimer > 0)
      {
         shootCooldownTimer = Mathf.Clamp(shootCooldownTimer - Time.deltaTime, 0, ShootCooldown);
         return;
      }

      var player = GameObject.FindWithTag("Player");
      var playerPosVector = player.transform.position - transform.position;
      if (player == null || playerPosVector.magnitude > SearchDistance)
         return;

      RaycastHit2D hit = Physics2D.Raycast(transform.position, playerPosVector);
      if(hit.collider != null && hit.collider.gameObject.tag == "Player")
         ShootAtPlayer(playerPosVector.normalized);
   }

   private void ShootAtPlayer(Vector2 direction)
   {
      shootCooldownTimer = ShootCooldown;
      var temp = Instantiate(Projectile, ProjectileOrigin.position, Quaternion.identity);
      var projectile = temp.GetComponent<Projectile>();
      projectile.Direction = direction;
      projectile.IgnoreTags.Add("Enemy");
   }
}
