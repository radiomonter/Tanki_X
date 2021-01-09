namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ReturnToBattleTextComponent : Component
    {
        public string DialogHeader { get; set; }

        public string DialogMainText { get; set; }

        public string DialogOk { get; set; }

        public string DialogNo { get; set; }
    }
}

