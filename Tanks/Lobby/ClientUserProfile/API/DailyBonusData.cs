namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class DailyBonusData
    {
        private Tanks.Lobby.ClientUserProfile.API.DailyBonusType dailyBonusType;

        public bool IsEpic() => 
            ((this.DailyBonusType == Tanks.Lobby.ClientUserProfile.API.DailyBonusType.CONTAINER) || ((this.DailyBonusType == Tanks.Lobby.ClientUserProfile.API.DailyBonusType.DETAIL) || (this.DailyBonusType == Tanks.Lobby.ClientUserProfile.API.DailyBonusType.XCRY))) || (this.DailyBonusType == Tanks.Lobby.ClientUserProfile.API.DailyBonusType.ENERGY);

        public long Code { get; set; }

        public Tanks.Lobby.ClientUserProfile.API.DailyBonusType DailyBonusType =>
            (this.dailyBonusType == Tanks.Lobby.ClientUserProfile.API.DailyBonusType.NONE) ? ((this.ContainerReward == null) ? ((this.DetailReward == null) ? ((this.CryAmount <= 0L) ? ((this.XcryAmount <= 0L) ? ((this.EnergyAmount <= 0L) ? Tanks.Lobby.ClientUserProfile.API.DailyBonusType.NONE : Tanks.Lobby.ClientUserProfile.API.DailyBonusType.ENERGY) : Tanks.Lobby.ClientUserProfile.API.DailyBonusType.XCRY) : Tanks.Lobby.ClientUserProfile.API.DailyBonusType.CRY) : Tanks.Lobby.ClientUserProfile.API.DailyBonusType.DETAIL) : Tanks.Lobby.ClientUserProfile.API.DailyBonusType.CONTAINER) : this.dailyBonusType;

        [ProtocolOptional]
        public long CryAmount { get; set; }

        [ProtocolOptional]
        public long XcryAmount { get; set; }

        [ProtocolOptional]
        public long EnergyAmount { get; set; }

        [ProtocolOptional]
        public DailyBonusGarageItemReward ContainerReward { get; set; }

        [ProtocolOptional]
        public DailyBonusGarageItemReward DetailReward { get; set; }
    }
}

