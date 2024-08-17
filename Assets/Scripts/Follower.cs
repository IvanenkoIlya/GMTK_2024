using UnityEngine;

public class Follower : MonoBehaviour
{
   public GameObject Target;
   public float MoveSpeed = 5f;
   public float FollowDistance = 3f;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      if(Target != null && Vector3.Distance(transform.position, Target.transform.position) > FollowDistance)
         transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, MoveSpeed * Time.deltaTime);
   }
}
