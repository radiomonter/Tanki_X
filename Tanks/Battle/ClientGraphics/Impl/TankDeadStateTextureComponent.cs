namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankDeadStateTextureComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Texture2D deadColorTexture;
        [SerializeField]
        private Texture2D deadEmissionTexture;
        [SerializeField]
        private AnimationCurve heatEmission;
        [SerializeField]
        private AnimationCurve whiteToHeat;
        [SerializeField]
        private AnimationCurve paintToHeat;

        public Date FadeStart { get; set; }

        public float LastFade { get; set; }

        public AnimationCurve HeatEmission =>
            this.heatEmission;

        public AnimationCurve WhiteToHeatTexture =>
            this.whiteToHeat;

        public AnimationCurve PaintTextureToWhiteHeat =>
            this.paintToHeat;

        public Texture2D DeadColorTexture =>
            this.deadColorTexture;

        public Texture2D DeadEmissionTexture =>
            this.deadEmissionTexture;
    }
}

