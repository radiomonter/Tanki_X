namespace Platform.Library.ClientUnityIntegration.Impl
{
    using System;
    using UnityEngine;

    public class GCTestBehaviour : MonoBehaviour
    {
        public float GCPeriod;
        private float nextGCTime;

        private void Update()
        {
            if (Time.time > this.nextGCTime)
            {
                GC.Collect();
                this.nextGCTime = Time.time + this.GCPeriod;
            }
        }
    }
}

