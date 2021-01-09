namespace Platform.Common.ClientECSCommon.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x150ad8fa5efL)]
    public class CreatedTimestampComponent : Component
    {
        public long Timestamp { get; set; }
    }
}

