namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine.EventSystems;

    public class EventSystemProviderComponent : ECSBehaviour, Component, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IEventSystemHandler
    {
        private EntityBehaviour entityBehaviour;

        private void Awake()
        {
            this.entityBehaviour = base.GetComponent<EntityBehaviour>();
        }

        private void ExecuteInFlow<T>(PointerEventData eventData) where T: EventSystemPointerEvent, new()
        {
            T eventInstance = Activator.CreateInstance<T>();
            eventInstance.PointerEventData = eventData;
            base.ScheduleEvent(eventInstance, this.entityBehaviour.Entity);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.ExecuteInFlow<EventSystemOnBeginDragEvent>(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.ExecuteInFlow<EventSystemOnDragEvent>(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.ExecuteInFlow<EventSystemOnEndDragEvent>(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            this.ExecuteInFlow<EventSystemOnPointerClickEvent>(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.ExecuteInFlow<EventSystemOnPointerDownEvent>(eventData);
        }
    }
}

