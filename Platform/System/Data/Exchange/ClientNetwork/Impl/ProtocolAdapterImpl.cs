namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientProtocol.Impl;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ProtocolAdapterImpl : ProtocolAdapter
    {
        private Protocol protocol;
        private CommandsCodec commandsCodec;
        private MemoryStreamData partialReceivedData;
        private static readonly Component[] EmptyComponents = new Component[0];

        public ProtocolAdapterImpl(Protocol protocol, CommandsCodec commandsCodec)
        {
            this.protocol = protocol;
            this.commandsCodec = commandsCodec;
            this.partialReceivedData = ClientProtocolInstancesCache.GetMemoryStreamDataInstance();
        }

        public void AddChunk(byte[] chunk, int length)
        {
            this.partialReceivedData.Write(chunk, 0, length);
        }

        private CommandPacket DecodePacket(ProtocolBuffer packetData)
        {
            List<Command> commandCollection = ClientNetworkInstancesCache.GetCommandCollection();
            while (packetData.Data.Position < packetData.Data.Length)
            {
                Command command = (Command) this.commandsCodec.Decode(packetData);
                if (ReferenceEquals(command.GetType(), typeof(InitTimeCommand)) || ReferenceEquals(command.GetType(), typeof(CloseCommand)))
                {
                    command.Execute(null);
                    continue;
                }
                if (!this.TrySplitCommands(command, commandCollection))
                {
                    commandCollection.Add(command);
                }
            }
            return ClientNetworkInstancesCache.GetCommandPacketInstance(commandCollection);
        }

        public MemoryStreamData Encode(CommandPacket commandPacket)
        {
            MemoryStreamData memoryStreamDataInstance = ClientProtocolInstancesCache.GetMemoryStreamDataInstance();
            ProtocolBuffer protocolBufferInstance = ClientProtocolInstancesCache.GetProtocolBufferInstance();
            List<Command> commands = commandPacket.Commands;
            int count = commands.Count;
            for (int i = 0; i < count; i++)
            {
                this.commandsCodec.Encode(protocolBufferInstance, commands[i]);
            }
            this.protocol.WrapPacket(protocolBufferInstance, memoryStreamDataInstance);
            ClientProtocolInstancesCache.ReleaseProtocolBufferInstance(protocolBufferInstance);
            return memoryStreamDataInstance;
        }

        public void FinalizeDecodedCommandPacket(CommandPacket commandPacket)
        {
            ClientNetworkInstancesCache.ReleaseCommandPacketWithCommandsCollection(commandPacket);
        }

        private bool IsPartialDataEmpty() => 
            this.partialReceivedData.Length == this.partialReceivedData.Position;

        private void KeepPartialReceivedData()
        {
            if (this.IsPartialDataEmpty())
            {
                this.partialReceivedData.Position = 0L;
                this.partialReceivedData.SetLength(0L);
            }
            else
            {
                long position = this.partialReceivedData.Position;
                long length = this.partialReceivedData.Length;
                MemoryStreamData partialReceivedData = this.partialReceivedData;
                this.partialReceivedData = ClientProtocolInstancesCache.GetMemoryStreamDataInstance();
                this.partialReceivedData.Write(partialReceivedData.GetBuffer(), (int) position, (int) (length - position));
                ClientProtocolInstancesCache.ReleaseMemoryStreamData(partialReceivedData);
            }
        }

        public bool TryDecode(out CommandPacket commandPacket)
        {
            commandPacket = null;
            if (this.partialReceivedData.Length == 0L)
            {
                return false;
            }
            ProtocolBuffer protocolBufferInstance = ClientProtocolInstancesCache.GetProtocolBufferInstance();
            bool flag = this.TryUnwrapPacket(protocolBufferInstance);
            if (flag)
            {
                this.KeepPartialReceivedData();
                commandPacket = this.DecodePacket(protocolBufferInstance);
            }
            ClientProtocolInstancesCache.ReleaseProtocolBufferInstance(protocolBufferInstance);
            return flag;
        }

        private bool TrySplitCommands(Command command, List<Command> commands)
        {
            if (!this.SplitShareCommand)
            {
                return false;
            }
            EntityShareCommand item = command as EntityShareCommand;
            if (item == null)
            {
                return false;
            }
            Component[] components = item.Components;
            item.Components = EmptyComponents;
            commands.Add(item);
            foreach (Component component in components)
            {
                ComponentAddCommand instance = ProtocolFlowInstances.GetInstance<ComponentAddCommand>();
                instance.Entity = item.GetOrCreateEntity();
                instance.Component = component;
                commands.Add(instance);
            }
            return true;
        }

        private bool TryUnwrapPacket(ProtocolBuffer packetData)
        {
            long position = this.partialReceivedData.Position;
            this.partialReceivedData.Position = 0L;
            if (this.protocol.UnwrapPacket(this.partialReceivedData, packetData))
            {
                return true;
            }
            this.partialReceivedData.Position = position;
            return false;
        }

        [Inject]
        public static Platform.Library.ClientProtocol.API.ClientProtocolInstancesCache ClientProtocolInstancesCache { get; set; }

        [Inject]
        public static Platform.System.Data.Exchange.ClientNetwork.API.ClientNetworkInstancesCache ClientNetworkInstancesCache { get; set; }

        [Inject]
        public static ProtocolFlowInstancesCache ProtocolFlowInstances { get; set; }

        public bool SplitShareCommand { get; set; }
    }
}

