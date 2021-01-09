namespace Platform.Library.ClientUnityIntegration.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class EngineBehaviour : MonoBehaviour
    {
        private static ApplicationFocusEvent applicationFocusEvent = new ApplicationFocusEvent();
        private static ViewportResizeEvent viewportResizeEvent = new ViewportResizeEvent();

        private void FixedUpdate()
        {
            FixedUpdateEvent.Instance.DeltaTime = Time.fixedDeltaTime;
            Flow flow = EngineServiceInternal.GetFlow();
            flow.TryInvoke(FixedUpdateEvent.Instance, typeof(FixedUpdateEventFireHandler));
            flow.TryInvoke(FixedUpdateEvent.Instance, typeof(FixedUpdateEventCompleteHandler));
        }

        private void OnApplicationFocus(bool isFocused)
        {
            applicationFocusEvent.IsFocused = isFocused;
            EngineEventSender.SendEventIntoEngine(EngineServiceInternal, applicationFocusEvent);
        }

        private void OnRectTransformDimensionsChange()
        {
            EngineEventSender.SendEventIntoEngine(EngineServiceInternal, viewportResizeEvent);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            TimeUpdateEvent.Instance.DeltaTime = deltaTime;
            EarlyUpdateEvent.Instance.DeltaTime = deltaTime;
            UpdateEvent.Instance.DeltaTime = deltaTime;
            Flow flow = EngineServiceInternal.GetFlow();
            flow.TryInvoke(TimeUpdateEvent.Instance, typeof(TimeUpdateFireHandler));
            flow.TryInvoke(TimeUpdateEvent.Instance, typeof(TimeUpdateCompleteHandler));
            flow.TryInvoke(EarlyUpdateEvent.Instance, typeof(EarlyUpdateFireHandler));
            flow.TryInvoke(EarlyUpdateEvent.Instance, typeof(EarlyUpdateCompleteHandler));
            flow.TryInvoke(UpdateEvent.Instance, typeof(UpdateEventFireHandler));
            flow.TryInvoke(UpdateEvent.Instance, typeof(UpdateEventCompleteHandler));
            EngineServiceInternal.GetFlow();
            EngineServiceInternal.DelayedEventManager.Update((double) Time.time);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineServiceInternal EngineServiceInternal { get; set; }
    }
}

