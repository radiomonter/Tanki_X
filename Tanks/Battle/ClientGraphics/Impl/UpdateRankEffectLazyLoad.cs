namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectLazyLoad : MonoBehaviour
    {
        public GameObject GO;
        public float TimeDelay = 0.3f;

        private void Awake()
        {
            this.GO.SetActive(false);
        }

        private void LazyEnable()
        {
            this.GO.SetActive(true);
        }

        private void OnDisable()
        {
            base.CancelInvoke("LazyEnable");
            this.GO.SetActive(false);
        }

        private void OnEnable()
        {
            base.Invoke("LazyEnable", this.TimeDelay);
        }
    }
}

