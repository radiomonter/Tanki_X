namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class TemperatureBehaviour : MonoBehaviour
    {
        public List<TemperatureVisualControllerComponent> temperatureControllers;
        private float temperature;
        private float tempDelta = 0.003f;
        private bool plusPressed;
        private bool minusPressed;

        public void Update()
        {
            bool keyDown = Input.GetKeyDown(KeyCode.KeypadPlus);
            bool keyUp = Input.GetKeyUp(KeyCode.KeypadPlus);
            this.plusPressed = keyDown || (this.plusPressed && !keyUp);
            if (this.plusPressed)
            {
                this.temperature += this.tempDelta;
            }
            bool flag3 = Input.GetKeyDown(KeyCode.KeypadMinus);
            bool flag4 = Input.GetKeyUp(KeyCode.KeypadMinus);
            this.minusPressed = flag3 || (this.minusPressed && !flag4);
            if (this.minusPressed)
            {
                this.temperature -= this.tempDelta;
            }
            this.temperature = Mathf.Clamp(this.temperature, -1f, 1f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.temperature = 0f;
            }
            foreach (TemperatureVisualControllerComponent component in this.temperatureControllers)
            {
                component.Temperature = this.temperature;
            }
        }
    }
}

