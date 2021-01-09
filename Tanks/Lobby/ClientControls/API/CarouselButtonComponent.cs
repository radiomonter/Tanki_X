namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public abstract class CarouselButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private EntityBehaviour entityBehaviour;
        private long carouselEntity;

        protected CarouselButtonComponent()
        {
        }

        public void Build(Entity btnEntity, long carouselEntity)
        {
            this.carouselEntity = carouselEntity;
            this.entityBehaviour.BuildEntity(btnEntity);
        }

        public void DestroyButton()
        {
            this.entityBehaviour.DestroyEntity();
        }

        public long CarouselEntity =>
            this.carouselEntity;
    }
}

