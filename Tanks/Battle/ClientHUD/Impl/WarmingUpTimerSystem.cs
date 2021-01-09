namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class WarmingUpTimerSystem : ECSSystem
    {
        [OnEventFire]
        public void HideWarmingUpTimer(NodeRemoveEvent e, RoundWarmingUpStateNode round, MainHUDNode hud)
        {
            hud.mainHUDTimers.HideWarmingUpTimer();
        }

        [OnEventFire]
        public void ShowWarmingUpTimer(NodeAddedEvent e, RoundWarmingUpStateNode round, MainHUDNode hud)
        {
            hud.mainHUDTimers.ShowWarmingUpTimer();
        }

        public class MainHUDNode : Node
        {
            public MainHUDComponent mainHUD;
            public MainHUDTimersComponent mainHUDTimers;
        }

        public class RoundWarmingUpStateNode : Node
        {
            public RoundStopTimeComponent roundStopTime;
            public RoundActiveStateComponent roundActiveState;
            public RoundWarmingUpStateComponent roundWarmingUpState;
        }
    }
}

