namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d52b55c9f2a612L)]
    public class DailyBonusCommonConfigComponent : Component, AttachToEntityListener
    {
        public static Entity DailyBonusConfig;

        public void AttachedToEntity(Entity entity)
        {
            DailyBonusConfig = entity;
        }

        public long ReceivingBonusIntervalInSeconds { get; set; }

        public long BattleCountToUnlockDailyBonuses { get; set; }

        public int PremiumTimeSpeedUp { get; set; }
    }
}

