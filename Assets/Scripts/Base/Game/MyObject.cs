namespace Base.Game
{
    using UnityEngine;

    public class MyObject : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Initialize();
            Registration();
        }

        protected virtual void Initialize()
        {

        }

        protected virtual void Registration()
        {
            Application.quitting += OnApplicationQuitting;
        }

        protected virtual void UnRegistration()
        {
            Application.quitting -= OnApplicationQuitting;
        }

        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }

        public virtual void DeActivate()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            UnRegistration();
        }

        protected virtual void OnApplicationQuit()
        {
            UnRegistration();
        }

        private void OnApplicationQuitting()
        {
            UnRegistration();
        }
    }
}