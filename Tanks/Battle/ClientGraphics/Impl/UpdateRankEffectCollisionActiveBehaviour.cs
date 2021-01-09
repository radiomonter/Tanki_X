namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectCollisionActiveBehaviour : MonoBehaviour
    {
        public bool IsReverse;
        public float TimeDelay;
        public bool IsLookAt;
        private UpdateRankEffectSettings effectSettings;

        private void effectSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e)
        {
            base.transform.LookAt(this.effectSettings.transform.position + e.Hit.normal);
        }

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

        private void Start()
        {
            this.GetEffectSettingsComponent(base.transform);
            if (!this.IsReverse)
            {
                this.effectSettings.RegistreActiveElement(base.gameObject, this.TimeDelay);
            }
            else
            {
                this.effectSettings.RegistreInactiveElement(base.gameObject, this.TimeDelay);
                base.gameObject.SetActive(false);
            }
            if (this.IsLookAt)
            {
                this.effectSettings.CollisionEnter += new EventHandler<UpdateRankCollisionInfo>(this.effectSettings_CollisionEnter);
            }
        }
    }
}

