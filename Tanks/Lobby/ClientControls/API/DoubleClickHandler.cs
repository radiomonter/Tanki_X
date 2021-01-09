namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    public class DoubleClickHandler : ECSBehaviour, Component, IPointerDownHandler, AttachToEntityListener, IEventSystemHandler
    {
        private Entity entity;
        private float delta = 0.2f;
        private float time;
        public FirstClickEvent FirstClick = new FirstClickEvent();

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if ((Time.realtimeSinceStartup - this.time) >= this.delta)
                {
                    this.time = Time.realtimeSinceStartup;
                }
                else
                {
                    base.ScheduleEvent<DoubleClickEvent>(this.entity);
                    this.time = 0f;
                }
            }
        }

        private void Update()
        {
            if ((this.time != 0f) && ((Time.realtimeSinceStartup - this.time) > this.delta))
            {
                this.time = 0f;
                this.FirstClick.Invoke();
            }
        }

        [Serializable]
        public class FirstClickEvent : UnityEvent
        {
        }
    }
}

