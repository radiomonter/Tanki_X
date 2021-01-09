namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientNetworkActivator : DefaultActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            ServerTimeServiceImpl service = new ServerTimeServiceImpl();
            ServiceRegistry.Current.RegisterService<ServerTimeService>(service);
            ServiceRegistry.Current.RegisterService<ServerTimeServiceInternal>(service);
            NetworkServiceImpl impl2 = ECSNetworkServerBuilder.Build(EngineServiceInternal, Protocol);
            ServiceRegistry.Current.RegisterService<ProtocolFlowInstancesCache>(new ProtocolFlowInstancesCache());
            ServiceRegistry.Current.RegisterService<NetworkService>(impl2);
        }

        public void RegisterSystemsAndTemplates()
        {
            TemplateRegistry.Register(typeof(ClientSessionTemplate));
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineServiceInternal EngineServiceInternal { get; set; }

        [Inject]
        public static Platform.Library.ClientProtocol.API.Protocol Protocol { get; set; }
    }
}

