namespace Platform.Common.ClientECSCommon.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x150ad941e50L)]
    public class OwnerComponent : Component
    {
        private Entity Owner { get; set; }
    }
}

