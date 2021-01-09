namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectOnStartSendCollision : MonoBehaviour
    {
        private UpdateRankEffectSettings effectSettings;
        private bool isInitialized;

        private void GetEffectSettingsComponent(Transform tr)
        {
            Transform parent = tr.parent;
            if (parent != null)
            {
                this.effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();
                if (this.effectSettings == null)
                {
                    this.GetEffectSettingsComponent(parent.transform);
                }
            }
        }

        private void OnEnable()
        {
            if (this.isInitialized)
            {
                this.effectSettings.OnCollisionHandler(new UpdateRankCollisionInfo());
            }
        }

        private void Start()
        {
            this.GetEffectSettingsComponent(base.transform);
            this.effectSettings.OnCollisionHandler(new UpdateRankCollisionInfo());
            this.isInitialized = true;
        }
    }
}

