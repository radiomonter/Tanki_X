namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectFadeInOutParticles : MonoBehaviour
    {
        private UpdateRankEffectSettings effectSettings;
        private ParticleSystem[] particles;
        private bool oldVisibleStat;

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
            this.particles = this.effectSettings.GetComponentsInChildren<ParticleSystem>();
            this.oldVisibleStat = this.effectSettings.IsVisible;
        }

        private void Update()
        {
            if (this.effectSettings.IsVisible != this.oldVisibleStat)
            {
                if (this.effectSettings.IsVisible)
                {
                    foreach (ParticleSystem system in this.particles)
                    {
                        if (this.effectSettings.IsVisible)
                        {
                            system.Play();
                            system.enableEmission = true;
                        }
                    }
                }
                else
                {
                    foreach (ParticleSystem system2 in this.particles)
                    {
                        if (!this.effectSettings.IsVisible)
                        {
                            system2.Stop();
                            system2.enableEmission = false;
                        }
                    }
                }
            }
            this.oldVisibleStat = this.effectSettings.IsVisible;
        }
    }
}

