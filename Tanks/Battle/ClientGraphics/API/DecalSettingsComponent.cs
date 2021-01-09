namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class DecalSettingsComponent : Component
    {
        private int maxCount;
        private float lifeTimeMultipler;
        private float maxDistanceToCamera;
        private bool enableDecals;
        private int maxDecalsForHammer;

        public float MaxDistanceToCamera
        {
            get => 
                this.maxDistanceToCamera;
            set => 
                this.maxDistanceToCamera = value;
        }

        public int MaxCount
        {
            get => 
                this.maxCount;
            set => 
                this.maxCount = value;
        }

        public float LifeTimeMultipler
        {
            get => 
                this.lifeTimeMultipler;
            set => 
                this.lifeTimeMultipler = value;
        }

        public bool EnableDecals
        {
            get => 
                this.enableDecals;
            set => 
                this.enableDecals = value;
        }

        public int MaxDecalsForHammer
        {
            get => 
                this.maxDecalsForHammer;
            set => 
                this.maxDecalsForHammer = value;
        }
    }
}

