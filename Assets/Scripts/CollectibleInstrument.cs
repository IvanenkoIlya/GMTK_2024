using UnityEngine;

public class CollectibleInstrument : MonoBehaviour
{
   public Sprite FollowerSprite;
   public GameObject Follower;
   public GameObject FollowerTarget;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {

   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      print("Collided!");
      if (collision.gameObject.tag == "Player")
      {
         var temp = Instantiate(Follower, transform.position, Quaternion.identity);
         var tempTarget = FollowerTarget.GetComponent<leader>().FollowerAlly;
         //Attempting to get the follower chain to work
         while (tempTarget != null)
         {
            FollowerTarget = FollowerTarget.GetComponent<leader>().FollowerAlly;
            tempTarget = FollowerTarget.GetComponent<leader>().FollowerAlly;
         }
         temp.GetComponent<Follower>().Target = FollowerTarget;
         FollowerTarget.GetComponent<leader>().FollowerAlly = temp;

         Destroy(gameObject);
      }
   }
}
