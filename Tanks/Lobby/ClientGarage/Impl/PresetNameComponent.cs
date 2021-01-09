namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15bd7d9516bL)]
    public class PresetNameComponent : SharedChangeableComponent
    {
        public string Name { get; set; }
    }
}

