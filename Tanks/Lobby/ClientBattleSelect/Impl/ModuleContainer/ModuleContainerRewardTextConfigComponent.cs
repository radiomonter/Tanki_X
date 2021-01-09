namespace Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x1607d2f3f99L)]
    public class ModuleContainerRewardTextConfigComponent : Component
    {
        public string OpenText { get; set; }

        public string WinText { get; set; }

        public string LooseText { get; set; }
    }
}

