namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4dda6733cbbc9L)]
    public class MinePrepareExplosionComponent : Component
    {
        public long PrepareDurationMS { get; set; }

        public Date PrepareExplosionStartTime { get; set; }
    }
}

