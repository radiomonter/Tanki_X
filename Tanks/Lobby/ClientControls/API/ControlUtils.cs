namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public static class ControlUtils
    {
        public static bool IsInteractable(this GameObject gameObject)
        {
            Selectable component = gameObject.GetComponent<Selectable>();
            CanvasGroup group = gameObject.GetComponent<CanvasGroup>();
            return (((group == null) || group.interactable) ? (((component != null) && component.enabled) && component.interactable) : false);
        }

        public static T SendEvent<T>(this MonoBehaviour behaviour, Entity entity = null) where T: Event, new() => 
            behaviour.SendEvent<T>(Activator.CreateInstance<T>(), entity);

        public static T SendEvent<T>(this MonoBehaviour behaviour, T evt, Entity entity = null) where T: Event
        {
            if (EngineService == null)
            {
                return null;
            }
            entity ??= ((EngineServiceInternal) EngineService).EntityStub;
            EngineService.Engine.ScheduleEvent(evt, entity);
            return evt;
        }

        public static void SetInteractable(this GameObject gameObject, bool interactable)
        {
            gameObject.GetComponent<Selectable>().interactable = interactable;
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }
    }
}

