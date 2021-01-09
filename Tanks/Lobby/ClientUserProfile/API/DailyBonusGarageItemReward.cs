namespace Tanks.Lobby.ClientUserProfile.API
{
    using System;
    using System.Runtime.CompilerServices;

    public class DailyBonusGarageItemReward
    {
        public DailyBonusGarageItemReward()
        {
        }

        public DailyBonusGarageItemReward(long marketItemId, long amount)
        {
            this.MarketItemId = marketItemId;
            this.Amount = amount;
        }

        private bool IsValid() => 
            this.Amount > 0L;

        public static bool IsValid(DailyBonusGarageItemReward reward) => 
            (reward != null) ? reward.IsValid() : false;

        public override string ToString() => 
            $"DailyBonusGarageItemReward: [MarketItemId = {this.MarketItemId}; Amount = {this.Amount}]";

        public long MarketItemId { get; set; }

        public long Amount { get; set; }
    }
}

