namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class DealItemContent : LocalizedControl, Component, ListItemContent, ContentWithOrder
    {
        private Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity;
        protected Date EndDate = new Date(float.PositiveInfinity);
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public ImageSkin banner;
        public TextMeshProUGUI price;
        public int order = 100;
        public bool canFillBigRow;
        public bool canFillSmallRow = true;

        protected virtual void FillFromEntity(Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity)
        {
        }

        private void OnEnable()
        {
            base.GetComponent<TextTimerComponent>().EndDate = this.EndDate;
        }

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
                this.entity = (Platform.Kernel.ECS.ClientEntitySystem.API.Entity) dataProvider;
                this.FillFromEntity(this.entity);
            }
        }

        public virtual void SetParent(Transform parent)
        {
            base.transform.SetParent(parent, false);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity =>
            this.entity;

        public int Order =>
            this.order;

        public bool CanFillBigRow =>
            this.canFillBigRow;

        public bool CanFillSmallRow =>
            this.canFillSmallRow;
    }
}

