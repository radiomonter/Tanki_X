namespace Platform.Library.ClientUnityIntegration.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PreciseTimeBehaviour : MonoBehaviour
    {
        private bool sendAfterFixedUpdateEvent;

        private void FixedUpdate()
        {
            this.SendAfterFixedUpdateEventIfNeed();
            InternalPreciseTime.FixedUpdate(Time.fixedDeltaTime);
            this.sendAfterFixedUpdateEvent = true;
        }

        private static void Invoke()
        {
            Flow current = Flow.Current;
            current.TryInvoke(AfterFixedUpdateEvent.Instance, typeof(AfterFixedUpdateEventFireHandler));
            current.TryInvoke(AfterFixedUpdateEvent.Instance, typeof(AfterFixedUpdateEventCompleteHandler));
        }

        private void SendAfterFixedUpdateEventIfNeed()
        {
            if (this.sendAfterFixedUpdateEvent)
            {
                InternalPreciseTime.AfterFixedUpdate();
                Flow flow = EngineServiceInternal.GetFlow();
                Invoke();
                this.sendAfterFixedUpdateEvent = false;
            }
        }

        private void Update()
        {
            this.SendAfterFixedUpdateEventIfNeed();
            InternalPreciseTime.Update(Time.deltaTime);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineServiceInternal EngineServiceInternal { get; set; }
    }
}

