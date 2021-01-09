namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class DelayedEntityBehaviourActivator : UnityAwareActivator<AutoCompleting>
    {
        public readonly List<EntityBehaviour> DelayedEntityBehaviours = new List<EntityBehaviour>(0x10);

        protected override void Activate()
        {
            if (!EngineService.IsRunning)
            {
                throw new Exception("Engine Service is not running!");
            }
            foreach (EntityBehaviour behaviour in this.DelayedEntityBehaviours)
            {
                if (behaviour.gameObject.activeInHierarchy)
                {
                    behaviour.CreateEntity();
                }
            }
            this.DelayedEntityBehaviours.Clear();
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

