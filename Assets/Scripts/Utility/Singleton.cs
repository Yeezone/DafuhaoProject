using UnityEngine;

namespace com.QH.QPGame.Utility
{
    public abstract class Singleton<T> where T : new()
    {
        private static T _instance = default(T);

        static Singleton()
        {
        }

        public static T GetInstance()
        {
            return Instance;
        }

        public static T Instance
        {
            get
            {
                if (Equals(_instance, default(T)))
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }

    public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour  
	{  
		private static T _instance;  
		private static bool _instanceInitialized;

		private static object _lock = new object();

        public static T GetInstance()
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[UnitySingleton] Instance '" + typeof(T) +
                                 "' already destroyed on application quit." +
                                 " Won't create again - returning null.");
                return null;
            }

            if (_instance != null && _instanceInitialized)
            {
                return _instance;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[UnitySingleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopenning the scene might fix it.");

                        _instanceInitialized = true;
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();


                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(UnitySingleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        //							Debug.Log("[UnitySingleton] An instance of " + typeof(T) +   
                        //							          " is needed in the scene, so '" + singleton +  
                        //							          "' was created with DontDestroyOnLoad.");  
                    }
                    else
                    {
                        Debug.Log("[UnitySingleton] Using instance already created: " + _instance.gameObject.name);
                    }

                    _instanceInitialized = true;
                }

                return _instance;
            } 
        }

		public static T Instance  
		{  
			get  
			{
                return GetInstance(); 
			}  
		}  
		
		private static bool applicationIsQuitting = false;  
		/// <summary>  
		/// When Unity quits, it destroys objects in a random order.  
		/// In principle, a Singleton is only destroyed when application quits.  
		/// If any script calls Instance after it have been destroyed,   
		///   it will create a buggy ghost object that will stay on the Editor scene  
		///   even after stopping playing the Application. Really bad!  
		/// So, this was made to be sure we're not creating that buggy ghost object.  
		/// </summary>  
		public void OnDestroy () 
		{  
			applicationIsQuitting = true;  
		}  

		public void OnApplicationQuit()
		{
			applicationIsQuitting = true; 
		}
	}  
}