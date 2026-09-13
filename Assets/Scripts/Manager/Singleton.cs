using UnityEngine;

public class Singleton<T> : BaseBehaviour where T : BaseBehaviour
{
   public static T Instance { get; private set; }


   protected override void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
         return;
      }


      Instance = this as T;
      DontDestroyOnLoad(gameObject);
   }
}