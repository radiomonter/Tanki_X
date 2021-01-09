namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(UnityEngine.CanvasGroup))]
    public class NameplateComponent : BehaviourComponent
    {
        private const float TEAM_NAMEPLATE_Y_OFFSET = 1.2f;
        public float yOffset = 2f;
        public float appearanceSpeed = 0.2f;
        public float disappearanceSpeed = 0.2f;
        public bool alwaysVisible;
        [SerializeField]
        private EntityBehaviour redHealthBarPrefab;
        [SerializeField]
        private EntityBehaviour blueHealthBarPrefab;
        [SerializeField]
        private Graphic colorProvider;
        private UnityEngine.CanvasGroup canvasGroup;
        private Camera _cachedCamera;

        public void AddBlueHealthBar(Entity entity)
        {
            this.AddHealthBar(this.blueHealthBarPrefab).BuildEntity(entity);
        }

        private EntityBehaviour AddHealthBar(EntityBehaviour prefab)
        {
            EntityBehaviour behaviour = Instantiate<EntityBehaviour>(prefab);
            behaviour.transform.SetParent(base.transform, false);
            this.yOffset = 1.2f;
            return behaviour;
        }

        public void AddRedHealthBar(Entity entity)
        {
            this.AddHealthBar(this.redHealthBarPrefab).BuildEntity(entity);
        }

        public UnityEngine.Color Color
        {
            get => 
                this.colorProvider.color;
            set => 
                this.colorProvider.color = value;
        }

        private UnityEngine.CanvasGroup CanvasGroup
        {
            get
            {
                if (this.canvasGroup == null)
                {
                    this.canvasGroup = base.GetComponent<UnityEngine.CanvasGroup>();
                }
                return this.canvasGroup;
            }
        }

        public float Alpha
        {
            get => 
                this.CanvasGroup.alpha;
            set => 
                this.CanvasGroup.alpha = value;
        }

        public Camera CachedCamera
        {
            get
            {
                if (!this._cachedCamera)
                {
                    this._cachedCamera = Camera.main;
                }
                return this._cachedCamera;
            }
        }
    }
}

