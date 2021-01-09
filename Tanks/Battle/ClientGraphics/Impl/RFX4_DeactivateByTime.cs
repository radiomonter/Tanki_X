namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RFX4_DeactivateByTime : MonoBehaviour
    {
        public float DeactivateTime = 3f;
        private bool canUpdateState;

        private void DeactivateThis()
        {
            base.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            this.canUpdateState = true;
        }

        private void Update()
        {
            if (this.canUpdateState)
            {
                this.canUpdateState = false;
                base.Invoke("DeactivateThis", this.DeactivateTime);
            }
        }
    }
}

