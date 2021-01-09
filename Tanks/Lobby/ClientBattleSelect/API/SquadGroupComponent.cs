namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x15ee7663148L)]
    public class SquadGroupComponent : GroupComponent
    {
        public SquadGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public SquadGroupComponent(long key) : base(key)
        {
        }
    }
}

