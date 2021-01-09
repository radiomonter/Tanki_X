namespace Platform.Library.ClientUnityIntegration.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ConfigEntityLoaderSystem : ECSSystem
    {
        [OnEventFire]
        public void LoadConfigEntity(ClientStartEvent e, Node node)
        {
            EntityLoader.LoadEntities(this);
        }

        [Inject]
        public static ConfigEntityLoader EntityLoader { get; set; }
    }
}

