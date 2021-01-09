namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class DynamicDecalProjectorComponent : BehaviourComponent
    {
        [SerializeField]
        private UnityEngine.Material material;
        [SerializeField]
        private UnityEngine.Color color = new UnityEngine.Color(0.5f, 0.5f, 0.5f, 0.5f);
        [SerializeField]
        private bool emit;
        [SerializeField]
        private float lifeTime = 20f;
        [SerializeField]
        private float halfSize = 1f;
        [SerializeField]
        private float randomKoef = 0.1f;
        [SerializeField]
        private bool randomRotation = true;
        [SerializeField]
        private int atlasHTilesCount = 1;
        [SerializeField]
        private int atlasVTilesCount = 1;
        [SerializeField]
        private float distance = 100f;
        [SerializeField, HideInInspector]
        private int[] surfaceAtlasPositions = new int[5];

        public UnityEngine.Material Material
        {
            get => 
                this.material;
            set => 
                this.material = value;
        }

        public UnityEngine.Color Color =>
            this.color;

        public bool Emmit =>
            this.emit;

        public float LifeTime =>
            this.lifeTime;

        public float HalfSize =>
            this.halfSize + Random.Range((float) 0f, (float) (this.halfSize * this.randomKoef));

        public Vector3 Up =>
            !this.randomRotation ? Vector3.up : Random.insideUnitSphere;

        public int AtlasHTilesCount =>
            this.atlasHTilesCount;

        public int AtlasVTilesCount =>
            this.atlasVTilesCount;

        public float Distance =>
            this.distance;

        public int[] SurfaceAtlasPositions
        {
            get => 
                this.surfaceAtlasPositions;
            set => 
                this.surfaceAtlasPositions = value;
        }
    }
}

