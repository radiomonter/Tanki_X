namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;

    public class LightContainer : MonoBehaviour
    {
        [SerializeField]
        private Light[] lights;
        public float maxLightIntensity = 2f;

        public void SetIntensity(float intensity)
        {
            int length = this.lights.Length;
            for (int i = 0; i < length; i++)
            {
                Light light = this.lights[i];
                light.intensity = intensity;
            }
        }
    }
}

