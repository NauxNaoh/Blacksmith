using UnityEngine;

namespace Naux.Patterns
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private bool dontDestroyOnLoad;

        private static T instance;
        public static T Instance => instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;

                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
                CustomAwake();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        protected virtual void CustomAwake() { }
    }
}