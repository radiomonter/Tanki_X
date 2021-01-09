namespace Platform.Library.ClientUnityIntegration.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Collections.Generic;

    public static class EngineEventSender
    {
        public static void SendEventIntoEngine(EngineServiceInternal engineServiceInternal, Event e)
        {
            IEnumerator<Entity> enumerator = engineServiceInternal.EntityRegistry.GetAllEntities().GetEnumerator();
            while (enumerator.MoveNext())
            {
                Flow.Current.SendEvent(e, enumerator.Current);
            }
        }
    }
}

