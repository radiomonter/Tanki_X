namespace Platform.Library.ClientUnityIntegration.API
{
    using System;
    using UnityEngine;

    public class DisabeRenderersByKeyBehaviour : MonoBehaviour
    {
        public KeyCode keyKode;
        private Renderer[] disabledRenderers = new Renderer[0];
        private bool renderersDisabled;

        private void DisableRenderers()
        {
            this.disabledRenderers = base.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in this.disabledRenderers)
            {
                renderer.enabled = false;
            }
        }

        private void EnabeRenderers()
        {
            foreach (Renderer renderer in this.disabledRenderers)
            {
                if (renderer)
                {
                    renderer.enabled = true;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(this.keyKode))
            {
                this.renderersDisabled = !this.renderersDisabled;
            }
            if (this.renderersDisabled)
            {
                this.DisableRenderers();
            }
            else
            {
                this.EnabeRenderers();
            }
        }
    }
}

