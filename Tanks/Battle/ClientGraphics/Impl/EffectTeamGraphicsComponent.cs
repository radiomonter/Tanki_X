namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class EffectTeamGraphicsComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Material redTeamMaterial;
        [SerializeField]
        private Material blueTeamMaterial;
        [SerializeField]
        private Material selfMaterial;

        public Material RedTeamMaterial
        {
            get => 
                this.redTeamMaterial;
            set => 
                this.redTeamMaterial = value;
        }

        public Material BlueTeamMaterial
        {
            get => 
                this.blueTeamMaterial;
            set => 
                this.blueTeamMaterial = value;
        }

        public Material SelfMaterial =>
            this.selfMaterial;
    }
}

