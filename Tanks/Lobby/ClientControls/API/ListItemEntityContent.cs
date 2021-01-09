namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class ListItemEntityContent : MonoBehaviour, ListItemContent
    {
        private Entity entity;

        protected ListItemEntityContent()
        {
        }

        protected abstract void FillFromEntity(Entity entity);
        public void Select()
        {
            if (!this.entity.HasComponent<SelectedListItemComponent>())
            {
                this.entity.AddComponent<SelectedListItemComponent>();
            }
            EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>(this.entity);
        }

        public void SetDataProvider(object dataProvider)
        {
            if (!ReferenceEquals(this.entity, dataProvider))
            {
                this.entity = (Entity) dataProvider;
                this.FillFromEntity(this.entity);
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

