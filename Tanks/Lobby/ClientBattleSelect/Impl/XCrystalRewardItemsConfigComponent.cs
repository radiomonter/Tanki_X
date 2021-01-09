namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d5479d32a9ced3L)]
    public class XCrystalRewardItemsConfigComponent : Component
    {
        public string SpriteUid { get; set; }

        public string Title { get; set; }
    }
}

