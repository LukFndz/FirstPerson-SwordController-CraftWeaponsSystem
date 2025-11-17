using UnityEngine;

namespace FP.Common
{
    /// <summary>
    /// This class is a generic MonoBehaviourSingleton that ensures there is only one instance of a MonoBehaviour-derived class.
    /// It can be used as a base class for creating singletons in Unity for specific MonoBehaviour types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this as T;
            }
        }
    }
}