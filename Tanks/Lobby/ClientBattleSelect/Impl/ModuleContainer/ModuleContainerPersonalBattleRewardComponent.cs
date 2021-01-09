namespace Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1607d3d3f4fL)]
    public class ModuleContainerPersonalBattleRewardComponent : Component
    {
        public long СontainerId { get; set; }
    }
}

