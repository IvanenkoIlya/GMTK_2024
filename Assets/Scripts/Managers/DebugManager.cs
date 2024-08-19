using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
   static DebugManager instance;

   public static DebugManager Instance
   {
      get
      {
         if (instance == null)
            instance = new DebugManager();
         return instance;
      }
   }

   private List<string> debugElements;

   public static void RegisterDebugParameter(string element)
   {
      Instance.debugElements.Add(element);
   }

   public static bool UnregisterDebugParameter(string element)
   {
      return Instance.debugElements.Remove(element);
   }
}
