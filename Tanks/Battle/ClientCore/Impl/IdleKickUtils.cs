namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public static class IdleKickUtils
    {
        public static float CalculateIdleTime(IdleCounterComponent idleCounter, Date? idleBeginTime) => 
            (idleBeginTime != null) ? (((float) (((idleCounter.SkipBeginTime == null) ? Date.Now : idleCounter.SkipBeginTime.Value) - idleBeginTime.Value)) - (((float) idleCounter.SkippedMillis) / 1000f)) : 0f;

        public static float CalculateTimeLeft(IdleCounterComponent idleCounter, IdleBeginTimeComponent idleBeginTime, IdleKickConfigComponent config)
        {
            float num = CalculateIdleTime(idleCounter, idleBeginTime.IdleBeginTime);
            float num2 = config.IdleKickTimeSec - num;
            return ((num2 >= 0f) ? num2 : 0f);
        }
    }
}

