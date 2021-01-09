namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class BattlePingSystem : ECSSystem
    {
        [OnEventFire]
        public void CancelEvent(NodeRemoveEvent e, SingleNode<BattlePingComponent> battleUser)
        {
            battleUser.component.PeriodicEventManager.Cancel();
        }

        [OnEventComplete]
        public void Deinit(NodeRemoveEvent e, SelfBattleUserNode battleUser)
        {
            battleUser.Entity.RemoveComponentIfPresent<BattlePingComponent>();
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SelfBattleUserNode battleUser)
        {
            BattlePingComponent component = new BattlePingComponent {
                PeriodicEventManager = base.NewEvent<PeriodicPingStartEvent>().Attach(battleUser).SchedulePeriodic(10f).Manager()
            };
            battleUser.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void OnPong(BattlePongEvent e, BattleUserWithPingNode battleUser)
        {
            BattlePingResultEvent eventInstance = new BattlePingResultEvent {
                ClientSendRealTime = e.ClientSendRealTime,
                ClientReceiveRealTime = UnityTime.realtimeSinceStartup
            };
            int ping = (int) (1000f * Mathf.Clamp((float) (UnityTime.realtimeSinceStartup - e.ClientSendRealTime), (float) 0f, (float) 10f));
            battleUser.battlePing.add(ping);
            base.ScheduleEvent(eventInstance, battleUser);
        }

        [OnEventFire]
        public void PeriodicPing(PeriodicPingStartEvent e, BattleUserWithPingNode battleUser)
        {
            float realtimeSinceStartup = UnityTime.realtimeSinceStartup;
            if (realtimeSinceStartup >= (battleUser.battlePing.LastPingTime + 5f))
            {
                battleUser.battlePing.LastPingTime = realtimeSinceStartup;
                BattlePingEvent eventInstance = new BattlePingEvent {
                    ClientSendRealTime = UnityTime.realtimeSinceStartup
                };
                base.ScheduleEvent(eventInstance, battleUser);
            }
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class BattleUserWithPingNode : Node
        {
            public BattlePingComponent battlePing;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
        }
    }
}

