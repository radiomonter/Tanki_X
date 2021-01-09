namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RFX4_StartDelay : MonoBehaviour
    {
        public GameObject ActivatedGameObject;
        public float Delay = 1f;

        private void ActivateGO()
        {
            this.ActivatedGameObject.SetActive(true);
        }

        private void OnDisable()
        {
            base.CancelInvoke("ActivateGO");
        }

        private void OnEnable()
        {
            this.ActivatedGameObject.SetActive(false);
            base.Invoke("ActivateGO", this.Delay);
        }
    }
}

