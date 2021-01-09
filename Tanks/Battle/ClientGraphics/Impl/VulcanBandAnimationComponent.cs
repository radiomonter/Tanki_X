namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class VulcanBandAnimationComponent : ECSBehaviour, Component
    {
        [SerializeField]
        private int materialIndex = 1;
        [SerializeField]
        private float speed = 1f;
        [SerializeField]
        private float bandCooldownSec = 0.2f;
        [SerializeField]
        private float partCount = 36f;
        [SerializeField]
        private string[] textureNames = new string[] { "_MainTex", "_PaintMap", "_FrostTex", "_HeatTex", "_SurfaceMap", "_BumpMap" };
        private Material bandMaterial;
        private float offset;
        private float stepLength;
        private Entity vulcanEntity;
        private float currentStepDistance;
        private float currentCooldown;
        private bool isEjectorEnabled;

        private void Awake()
        {
            base.enabled = false;
        }

        public void InitBand(Renderer renderer, Entity entity)
        {
            this.vulcanEntity = entity;
            this.bandMaterial = renderer.materials[this.materialIndex];
            this.stepLength = 1f / this.partCount;
            this.offset = 0f;
        }

        private void OnEnable()
        {
            this.currentStepDistance = 0f;
            this.currentCooldown = 0f;
            this.isEjectorEnabled = true;
        }

        private void ProvideCaseEjectionEvent(Engine engine)
        {
            base.NewEvent<CartridgeCaseEjectionEvent>().Attach(this.vulcanEntity).Schedule();
        }

        public void StartBandAnimation()
        {
            base.enabled = true;
        }

        public void StopBandAnimation()
        {
            base.enabled = false;
        }

        private void Update()
        {
            if (this.currentCooldown > 0f)
            {
                this.currentCooldown -= Time.deltaTime;
            }
            else
            {
                if (this.isEjectorEnabled)
                {
                    this.isEjectorEnabled = false;
                    this.ProvideCaseEjectionEvent(ECSBehaviour.EngineService.Engine);
                }
                float num = this.speed * Time.deltaTime;
                this.currentStepDistance += num;
                this.offset += num;
                this.offset = Mathf.Repeat(this.offset, 1f);
                int length = this.textureNames.Length;
                for (int i = 0; i < length; i++)
                {
                    this.bandMaterial.SetTextureOffset(this.textureNames[i], new Vector2(this.offset, 0f));
                }
                if (Mathf.Abs(this.currentStepDistance) >= this.stepLength)
                {
                    this.currentStepDistance = 0f;
                    this.currentCooldown = this.bandCooldownSec;
                    this.isEjectorEnabled = true;
                }
            }
        }
    }
}

