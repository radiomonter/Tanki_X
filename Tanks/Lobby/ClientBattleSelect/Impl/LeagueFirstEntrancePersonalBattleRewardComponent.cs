namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1606e6ae31bL)]
    public class LeagueFirstEntrancePersonalBattleRewardComponent : Component
    {
        public Entity PersonalOffer { get; set; }
    }
}

