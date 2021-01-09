namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1606d895100L)]
    public class XCrystalBonusPersonalRewardComponent : Component
    {
        public XCrystalBonusActivationReason ActivationReason { get; set; }

        public double Multiplier { get; set; }
    }
}

