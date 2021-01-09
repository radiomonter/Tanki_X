namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d5283f5a7e23f7L)]
    public class UserDailyBonusReceivedRewardsComponent : Component
    {
        public List<long> ReceivedRewards { get; set; }
    }
}

