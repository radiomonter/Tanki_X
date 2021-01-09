namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class ClientUnityIntegrationUtils
    {
        public static void CollectComponents(this GameObject gameObject, Entity entity)
        {
            foreach (Component component in gameObject.GetComponents(typeof(Component)))
            {
                ((EntityInternal) entity).AddComponent((Component) component);
            }
        }

        public static void CollectComponentsInChildren(this GameObject gameObject, Entity entity)
        {
            foreach (Component component in gameObject.GetComponentsInChildren(typeof(Component)))
            {
                ((EntityInternal) entity).AddComponent((Component) component);
            }
        }

        public static void ExecuteInFlow(Consumer<Engine> action)
        {
            if (!HasEngine())
            {
                Debug.LogError("Engine does not exist");
            }
            else
            {
                Flow.Current.ScheduleWith(action);
            }
        }

        public static bool HasEngine() => 
            !ReferenceEquals(EngineServiceInternal, null);

        public static bool HasWorkingEngine() => 
            (EngineServiceInternal != null) && EngineServiceInternal.IsRunning;

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineServiceInternal EngineServiceInternal { get; set; }
    }
}

