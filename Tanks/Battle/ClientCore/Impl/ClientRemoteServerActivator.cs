namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.Impl;
    using Platform.Library.ClientUnityIntegration;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using Platform.System.Data.Exchange.ClientNetwork.Impl;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class ClientRemoteServerActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        private NetworkServiceImpl networkService;
        [CompilerGenerated]
        private static Func<string, int> <>f__mg$cache0;

        protected override void Activate()
        {
            TimeServiceImpl service = new TimeServiceImpl();
            ServiceRegistry.Current.RegisterService<TimeService>(service);
            ServerTimeService.OnInitServerTime += new Action<long>(service.InitServerTime);
            Protocol.RegisterCodecForType<Movement>(new MovementCodec());
            Protocol.RegisterCodecForType<MoveCommand>(new MoveCommandCodec());
            Protocol.RegisterCodecForType<Date>(new DateCodec());
            string host = InitConfiguration.Config.Host;
            string[] source = new string[] { InitConfiguration.Config.AcceptorPort };
            this.PrefetchSocketPolicy(host);
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<string, int>(Convert.ToInt32);
            }
            base.gameObject.AddComponent<ServerConnectionBehaviour>().OpenConnection(host, source.Select<string, int>(<>f__mg$cache0).ToArray<int>());
        }

        private void PrefetchSocketPolicy(string hostName)
        {
        }

        public void RegisterSystemsAndTemplates()
        {
            EngineService.RegisterSystem(new TimeSyncSystem());
            EngineService.RegisterSystem(new TankMovementReceiverSystem());
            EngineService.RegisterSystem(new TankMovementSenderSystem());
            EngineService.RegisterSystem(new WallhackSystem());
            EngineService.RegisterSystem(new FlyingTankSystem());
            EngineService.RegisterSystem(new TankAutopilotControllerSystem());
            EngineService.RegisterSystem(new TankAutopilotWeaponControllerSystem());
            EngineService.RegisterSystem(new TankAutopilotNavigationSystem());
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [Inject]
        public static Platform.Library.ClientProtocol.API.Protocol Protocol { get; set; }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        [Inject]
        public static Platform.System.Data.Exchange.ClientNetwork.API.ServerTimeService ServerTimeService { get; set; }
    }
}

