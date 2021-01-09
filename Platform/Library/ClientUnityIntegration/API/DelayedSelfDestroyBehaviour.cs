namespace Platform.Library.ClientUnityIntegration.API
{
    using System;
    using UnityEngine;

    public class DelayedSelfDestroyBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float delay;
        private float destroyTime;

        private void Start()
        {
            this.destroyTime = Time.time + this.delay;
        }

        private void Update()
        {
            if (Time.time > this.destroyTime)
            {
                Destroy(base.gameObject);
            }
        }

        public float Delay
        {
            get => 
                this.delay;
            set => 
                this.delay = value;
        }
    }
}

