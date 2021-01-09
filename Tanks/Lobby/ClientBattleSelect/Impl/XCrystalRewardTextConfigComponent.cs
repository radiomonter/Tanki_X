namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d546f50c8caabfL)]
    public class XCrystalRewardTextConfigComponent : Component
    {
        public IDictionary<XCrystalBonusActivationReason, string> Title { get; set; }

        public IDictionary<XCrystalBonusActivationReason, string> Description { get; set; }

        public string PurchaseText { get; set; }
    }
}

