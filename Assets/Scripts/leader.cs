using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
   public List<GameObject> Followers = new List<GameObject>();

   public void AssignFollower(GameObject follower)
   {
      Followers.Add(follower);
   }
}
