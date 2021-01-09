namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientProtocol.Impl;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ECSNetworkServerBuilder
    {
        public static NetworkServiceImpl Build(EngineServiceInternal engineServiceInternal, Protocol protocol)
        {
            ServiceRegistry.Current.RegisterService<ClientProtocolInstancesCache>(new ClientProtocolInstancesCacheImpl());
            ServiceRegistry.Current.RegisterService<ClientNetworkInstancesCache>(new ClientNetworkInstancesCacheImpl());
            ComponentAndEventRegistrator componentAndEventRegistrator = new ComponentAndEventRegistrator(engineServiceInternal, protocol);
            SharedEntityRegistry service = new SharedEntityRegistryImpl(engineServiceInternal);
            ServiceRegistry.Current.RegisterService<SharedEntityRegistry>(service);
            CommandsCodecImpl impl = new CommandsCodecImpl(TemplateRegistry);
            impl.Init(protocol);
            ServiceRegistry.Current.RegisterService<CommandsCodec>(impl);
            return Build(engineServiceInternal, protocol, componentAndEventRegistrator, service, impl);
        }

        public static NetworkServiceImpl Build(EngineServiceInternal engineServiceInternal, Protocol protocol, ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry, CommandsCodec commandsCodec)
        {
            NetworkServiceImpl networkService = new NetworkServiceImpl(new ProtocolAdapterImpl(protocol, commandsCodec), new TcpSocketImpl());
            CommandsSender sender1 = new CommandsSender(engineServiceInternal, networkService, componentAndEventRegistrator, entityRegistry);
            return networkService;
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

