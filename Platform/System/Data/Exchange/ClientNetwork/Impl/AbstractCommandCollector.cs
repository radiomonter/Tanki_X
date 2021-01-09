namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AbstractCommandCollector
    {
        private readonly CommandCollector commandCollector;

        public AbstractCommandCollector(CommandCollector commandCollector)
        {
            this.commandCollector = commandCollector;
        }

        protected void AddCommand(Command command)
        {
            this.commandCollector.Add(command);
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }

        [Inject]
        public static ProtocolFlowInstancesCache ProtcolCache { get; set; }

        public bool DecodeStage { get; set; }
    }
}

