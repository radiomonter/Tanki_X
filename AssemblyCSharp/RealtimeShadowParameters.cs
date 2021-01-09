namespace AssemblyCSharp
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode]
    public class RealtimeShadowParameters : MonoBehaviour
    {
        public Color shadowColor;
        public float shadowStrength;

        private void OnEnable()
        {
            Shader.SetGlobalColor("_ShadowMixColor", this.shadowColor);
            Shader.SetGlobalFloat("_ShadowMixStrength", this.shadowStrength);
        }
    }
}

