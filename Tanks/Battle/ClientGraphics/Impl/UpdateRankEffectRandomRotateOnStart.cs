namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectRandomRotateOnStart : MonoBehaviour
    {
        public Vector3 NormalizedRotateVector = new Vector3(0f, 1f, 0f);
        private Transform t;
        private bool isInitialized;

        private void OnEnable()
        {
            if (this.isInitialized)
            {
                this.t.Rotate(this.NormalizedRotateVector * Random.Range(0, 360));
            }
        }

        private void Start()
        {
            this.t = base.transform;
            this.t.Rotate(this.NormalizedRotateVector * Random.Range(0, 360));
            this.isInitialized = true;
        }
    }
}

