namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CloseCommand : Command
    {
        public void Execute(Engine engine)
        {
            ServerConnectionCloseReasonEvent eventInstance = new ServerConnectionCloseReasonEvent {
                Reason = this.Reason
            };
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public string Reason { get; set; }
    }
}

