namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class InitTimeCommand : Command
    {
        public void Execute(Engine engine)
        {
            ServerTimeService.InitialServerTime = this.ServerTime;
        }

        [ProtocolTransient, Inject]
        public static ServerTimeServiceInternal ServerTimeService { get; set; }

        public long ServerTime { get; set; }
    }
}

