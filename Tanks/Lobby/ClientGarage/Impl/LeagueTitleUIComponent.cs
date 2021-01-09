namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class LeagueTitleUIComponent : BehaviourComponent
    {
        private Entity leagueEntity;
        [SerializeField]
        private TextMeshProUGUI name;
        [SerializeField]
        private ImageSkin icon;

        public void Init(Entity entity)
        {
            if (entity.HasComponent<LeagueTitleUIComponent>())
            {
                entity.RemoveComponent<LeagueTitleUIComponent>();
            }
            entity.AddComponent(this);
            this.leagueEntity = entity;
        }

        private void OnDestroy()
        {
            if (ClientUnityIntegrationUtils.HasEngine())
            {
                this.RemoveFromEntity();
            }
        }

        private void RemoveFromEntity()
        {
            if ((this.leagueEntity != null) && this.leagueEntity.HasComponent<LeagueTitleUIComponent>())
            {
                this.leagueEntity.RemoveComponent<LeagueTitleUIComponent>();
            }
        }

        public Entity LeagueEntity =>
            this.leagueEntity;

        public string Name
        {
            set => 
                this.name.text = value;
        }

        public string Icon
        {
            set => 
                this.icon.SpriteUid = value;
        }
    }
}

