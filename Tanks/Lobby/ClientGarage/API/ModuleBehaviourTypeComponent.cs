namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4bd4a3b2defe2L)]
    public class ModuleBehaviourTypeComponent : Component
    {
        public ModuleBehaviourType Type { get; set; }
    }
}

