namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(EntityBehaviour))]
    public class RightMouseButtonClickSender : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                EngineService.Engine.ScheduleEvent(new RightMouseButtonClickEvent(), base.GetComponent<EntityBehaviour>().Entity);
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

