namespace Tanks.Lobby.ClientUserProfile.API
{
    using System;
    using Tanks.Lobby.ClientEntrance.API;

    public static class DailyBonusUtil
    {
        public static DailyBonusCycleComponent GetCurrentCycle() => 
            (SelfUserComponent.SelfUser.GetComponent<UserDailyBonusCycleComponent>().CycleNumber <= 0L) ? ((DailyBonusCycleComponent) DailyBonusCommonConfigComponent.DailyBonusConfig.GetComponent<DailyBonusFirstCycleComponent>()) : ((DailyBonusCycleComponent) DailyBonusCommonConfigComponent.DailyBonusConfig.GetComponent<DailyBonusEndlessCycleComponent>());

        public static int GetCurrentZoneIndex() => 
            (int) SelfUserComponent.SelfUser.GetComponent<UserDailyBonusZoneComponent>().ZoneNumber;

        public static int GetFirstIndexInZone(DailyBonusCycleComponent dailyBonusCycleComponent, int zoneIndex) => 
            (zoneIndex != 0) ? (dailyBonusCycleComponent.Zones[zoneIndex - 1] + 1) : 0;

        public static int GetLastIndexInZone(DailyBonusCycleComponent dailyBonusCycleComponent, int zoneIndex) => 
            dailyBonusCycleComponent.Zones[zoneIndex];
    }
}

