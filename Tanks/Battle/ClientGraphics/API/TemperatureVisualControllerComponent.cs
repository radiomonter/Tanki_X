namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class TemperatureVisualControllerComponent : MonoBehaviour, Component
    {
        public Renderer renderer;
        public List<TemperatureChangeListener> listeners = new List<TemperatureChangeListener>();
        private float temperature;

        public void Reset()
        {
            this.temperature = 0f;
            if (this.renderer != null)
            {
                TankMaterialsUtil.SetTemperature(this.renderer, this.temperature);
            }
            int count = this.listeners.Count;
            for (int i = 0; i < count; i++)
            {
                this.listeners[i].TemperatureChanged(this.temperature);
            }
        }

        public float Temperature
        {
            get => 
                this.temperature;
            set
            {
                if (Math.Abs((float) (this.temperature - value)) >= 0.0001)
                {
                    this.temperature = value;
                    if (this.renderer != null)
                    {
                        TankMaterialsUtil.SetTemperature(this.renderer, this.temperature);
                    }
                    int count = this.listeners.Count;
                    for (int i = 0; i < count; i++)
                    {
                        this.listeners[i].TemperatureChanged(this.temperature);
                    }
                }
            }
        }
    }
}

