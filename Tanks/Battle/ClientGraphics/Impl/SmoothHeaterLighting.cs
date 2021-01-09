namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class SmoothHeaterLighting : SmoothHeater
    {
        private readonly LightContainer lightContainer;

        public SmoothHeaterLighting(int burningTimeInMs, Material temperatureMaterial, MonoBehaviour updater, LightContainer lightContainer) : base(burningTimeInMs, temperatureMaterial, updater)
        {
            this.lightContainer = lightContainer;
        }

        protected override void FinalizeCooling()
        {
            base.FinalizeCooling();
            this.lightContainer.gameObject.SetActive(false);
        }

        public override void Heat()
        {
            this.lightContainer.SetIntensity(0f);
            this.lightContainer.gameObject.SetActive(true);
            base.Heat();
        }

        protected override void UpdateBurning()
        {
            base.UpdateBurning();
            this.UpdateLightsIntencity();
        }

        protected override void UpdateCooling()
        {
            base.UpdateCooling();
            this.UpdateLightsIntencity();
        }

        private void UpdateLightsIntencity()
        {
            this.lightContainer.SetIntensity(base.temperature * this.lightContainer.maxLightIntensity);
        }
    }
}

