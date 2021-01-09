namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x168316b70f3L)]
    public class FractionsCompetitionRewardNotificationComponent : Component
    {
        public long WinnerFractionId { get; set; }

        public long CrysForWin { get; set; }

        public Dictionary<long, int> Rewards { get; set; }
    }
}

