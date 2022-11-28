using UnityEngine;

namespace Overspace.Pattern.Singleton
{
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"There is no {typeof(T).Name} instance but is being accessed.");
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = GetComponent<T>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError($"Duplicate {typeof(T).Name} instance found, destroying.");
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}