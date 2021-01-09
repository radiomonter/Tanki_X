namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using log4net;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CommandsSender : FlowListener
    {
        private readonly CommandCollector commandCollector;
        private readonly NetworkService networkService;
        private readonly SharedEntityRegistry entityRegistry;
        private ILog logger;

        public CommandsSender(EngineService engineService, NetworkService networkService, ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry)
        {
            this.networkService = networkService;
            this.entityRegistry = entityRegistry;
            this.commandCollector = new CommandCollector();
            this.logger = LoggerProvider.GetLogger(this);
            EventCommandCollector eventListener = new EventCommandCollector(this.commandCollector, componentAndEventRegistrator, entityRegistry);
            ComponentCommandCollector componentListener = new ComponentCommandCollector(this.commandCollector, componentAndEventRegistrator, entityRegistry);
            engineService.AddFlowListener(this);
            engineService.AddComponentListener(componentListener);
            engineService.AddEventListener(eventListener);
        }

        public void OnFlowClean()
        {
        }

        public void OnFlowFinish()
        {
            List<Command> commands = this.commandCollector.Commands;
            if (commands.Count > 0)
            {
                List<Command> commandCollection = ClientNetworkInstancesCache.GetCommandCollection();
                int count = commands.Count;
                int num2 = 0;
                while (true)
                {
                    if (num2 >= count)
                    {
                        if (commandCollection.Count > 0)
                        {
                            CommandPacket commandPacketInstance = ClientNetworkInstancesCache.GetCommandPacketInstance(commandCollection);
                            this.networkService.SendCommandPacket(commandPacketInstance);
                        }
                        this.commandCollector.Clear();
                        break;
                    }
                    Command command = commands[num2];
                    this.logger.InfoFormat("Out {0}", command);
                    commandCollection.Add(command);
                    num2++;
                }
            }
        }

        [Inject]
        public static Platform.System.Data.Exchange.ClientNetwork.API.ClientNetworkInstancesCache ClientNetworkInstancesCache { get; set; }
    }
}

