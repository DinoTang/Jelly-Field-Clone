using UnityEngine;

public class Singleton<T> : BaseBehaviour where T : BaseBehaviour
{
   private static T instance;
   public static T Instance => instance;


   protected override void Awake()
   {
      if (instance != null && instance != this)
      {
         Destroy(gameObject);
         return;
      }


      instance = this as T;
      DontDestroyOnLoad(gameObject);
   }
}