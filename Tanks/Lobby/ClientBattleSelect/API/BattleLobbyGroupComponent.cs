namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x15c53ac5d9bL)]
    public class BattleLobbyGroupComponent : GroupComponent
    {
        public BattleLobbyGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public BattleLobbyGroupComponent(long key) : base(key)
        {
        }
    }
}

