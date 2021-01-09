namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine.EventSystems;

    public class BattleChatFocusWatcherComponent : ECSBehaviour, Component, AttachToEntityListener, DetachFromEntityListener, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        private Entity entity;

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        public void DetachedFromEntity(Entity entity)
        {
            this.entity = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            base.ScheduleEvent<PointEnterToBattleChatScrollViewEvent>(this.entity);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            base.ScheduleEvent<PointExitFromBattleChatScrollViewEvent>(this.entity);
        }
    }
}

