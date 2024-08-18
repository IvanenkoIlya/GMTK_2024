using UnityEngine;

public class CollectibleInstrument : MonoBehaviour
{
   public GameObject FollowerPrefab;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start() { }

   // Update is called once per frame
   void Update() { }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if(collision.gameObject.CompareTag("Player"))
      {
         var target = collision.gameObject;

         while(target.GetComponent<Leader>().Followers.Count > 0)
            target = target.GetComponent<Leader>().Followers[0];

         var follower = Instantiate(FollowerPrefab, transform.position, Quaternion.identity);
         target.GetComponent<Leader>().AssignFollower(follower);
         follower.GetComponent<Follower>().Target = target;

         Destroy(gameObject);
      }
   }
}
