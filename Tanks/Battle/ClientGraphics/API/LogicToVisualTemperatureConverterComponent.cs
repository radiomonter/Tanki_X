namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class LogicToVisualTemperatureConverterComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private AnimationCurve logicTemperatureToVisualTemperature;

        public float ConvertToVisualTemperature(float logicTemperature) => 
            this.logicTemperatureToVisualTemperature.Evaluate(logicTemperature);
    }
}

