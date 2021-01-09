namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class SmoothHeater
    {
        private readonly Material temperatureMaterial;
        private readonly int burningTimeInMs;
        private bool burningInProgress;
        private bool coolingInProgress;
        private MonoBehaviour updater;
        protected float temperature;
        private float startProcessTime;

        public SmoothHeater(int burningTimeInMs, Material temperatureMaterial, MonoBehaviour updater)
        {
            this.burningTimeInMs = burningTimeInMs;
            this.temperatureMaterial = temperatureMaterial;
            this.updater = updater;
        }

        public void Cool()
        {
            this.coolingInProgress = true;
            this.burningInProgress = false;
            this.startProcessTime = Time.time;
            this.updater.enabled = true;
        }

        protected void FinalizeBurning()
        {
            this.burningInProgress = false;
            this.updater.enabled = false;
        }

        protected virtual void FinalizeCooling()
        {
            this.coolingInProgress = false;
            this.updater.enabled = false;
        }

        public virtual void Heat()
        {
            this.burningInProgress = true;
            this.coolingInProgress = false;
            this.startProcessTime = Time.time;
            this.updater.enabled = true;
        }

        public void Update()
        {
            if (this.burningInProgress)
            {
                this.UpdateBurning();
                if (this.temperature.Equals((float) 1f))
                {
                    this.FinalizeBurning();
                }
            }
            if (this.coolingInProgress)
            {
                this.UpdateCooling();
                if (this.temperature.Equals((float) 0f))
                {
                    this.FinalizeCooling();
                }
            }
        }

        protected virtual void UpdateBurning()
        {
            if (this.temperatureMaterial)
            {
                this.temperature = Mathf.Clamp01(((Time.time - this.startProcessTime) * 1000f) / ((float) this.burningTimeInMs));
                this.temperatureMaterial.SetFloat(TankMaterialPropertyNames.TEMPERATURE_ID, this.temperature);
            }
        }

        protected virtual void UpdateCooling()
        {
            if (this.temperatureMaterial)
            {
                this.temperature = 1f - Mathf.Clamp01(((Time.time - this.startProcessTime) * 1000f) / ((float) this.burningTimeInMs));
                this.temperatureMaterial.SetFloat(TankMaterialPropertyNames.TEMPERATURE_ID, this.temperature);
            }
        }
    }
}

