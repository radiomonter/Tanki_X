namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class HealthFeedbackMapEffectMaterialComponent : BehaviourComponent
    {
        [SerializeField]
        private Material sourceMaterial;
        [SerializeField]
        private float intensitySpeed = 2f;

        public float IntensitySpeed =>
            this.intensitySpeed;

        public Material SourceMaterial =>
            this.sourceMaterial;
    }
}

