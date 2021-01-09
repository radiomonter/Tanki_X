namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class TimeSyncSystem : ECSSystem
    {
        [OnEventFire]
        public void AddComponent(NodeAddedEvent e, SingleNode<ClientSessionComponent> session)
        {
            session.Entity.AddComponent<ClientTimeComponent>();
        }

        [OnEventFire]
        public void Ping(PingEvent e, SessionNode session)
        {
            float realtimeSinceStartup = UnityTime.realtimeSinceStartup;
            base.Log.InfoFormat("Ping e={0} pongCommandClientRealTime={1}", e, realtimeSinceStartup);
            PongEvent eventInstance = new PongEvent {
                PongCommandClientRealTime = realtimeSinceStartup,
                CommandId = e.CommandId
            };
            base.ScheduleEvent(eventInstance, session);
            session.clientTime.PingServerTime = e.ServerTime;
            session.clientTime.PingClientRealTime = realtimeSinceStartup;
        }

        [OnEventFire]
        public void PingResult(PingResultEvent e, SessionNode session)
        {
            float pingClientRealTime = session.clientTime.PingClientRealTime;
            long num3 = e.ServerTime - ((long) (((pingClientRealTime + UnityTime.realtimeSinceStartup) * 1000f) / 2f));
            base.Log.InfoFormat("PingResult newDiffToServer={0} e={1}", num3, e);
            TimeService.DiffToServer = num3;
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        [Inject]
        public static Tanks.Battle.ClientCore.API.TimeService TimeService { get; set; }

        public class SessionNode : Node
        {
            public ClientSessionComponent clientSession;
            public ClientTimeComponent clientTime;
        }
    }
}

