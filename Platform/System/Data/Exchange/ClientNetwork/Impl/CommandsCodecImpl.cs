namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CommandsCodecImpl : CommandsCodec, Codec
    {
        private Protocol protocol;
        private Codec commandCodeCodec;
        private Dictionary<CommandCode, Type> typeByCode = new Dictionary<CommandCode, Type>();
        private Dictionary<Type, CommandCode> codeByType = new Dictionary<Type, CommandCode>();
        private TemplateRegistry templateRegistry;

        public CommandsCodecImpl(TemplateRegistry templateRegistry)
        {
            this.templateRegistry = templateRegistry;
            this.RegisterCommand<EntityShareCommand>(CommandCode.EntityShare);
            this.RegisterCommand<EntityUnshareCommand>(CommandCode.EntityUnshare);
            this.RegisterCommand<ComponentAddCommand>(CommandCode.ComponentAdd);
            this.RegisterCommand<ComponentRemoveCommand>(CommandCode.ComponentRemove);
            this.RegisterCommand<ComponentChangeCommand>(CommandCode.ComponentChange);
            this.RegisterCommand<SendEventCommand>(CommandCode.SendEvent);
            this.RegisterCommand<InitTimeCommand>(CommandCode.InitTime);
            this.RegisterCommand<CloseCommand>(CommandCode.Close);
        }

        public object Decode(ProtocolBuffer protocolBuffer)
        {
            Type type;
            CommandCode key = (CommandCode) this.commandCodeCodec.Decode(protocolBuffer);
            if (!this.typeByCode.TryGetValue(key, out type))
            {
                throw new UnknownCommandException(key);
            }
            object instance = ProtocolFlowInstances.GetInstance(type);
            this.protocol.GetCodec(type).DecodeToInstance(protocolBuffer, instance);
            return instance;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            throw new NotImplementedException();
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            Command command = (Command) data;
            Type type = command.GetType();
            CommandCode code = this.codeByType[type];
            Codec codec = this.protocol.GetCodec(type);
            this.commandCodeCodec.Encode(protocolBuffer, code);
            codec.Encode(protocolBuffer, command);
        }

        public void Init(Protocol protocol)
        {
            this.protocol = protocol;
            this.commandCodeCodec = protocol.GetCodec(typeof(CommandCode));
            protocol.RegisterCodecForType<TemplateAccessor>(new TemplateAccessorCodec(this.templateRegistry));
            protocol.RegisterCodecForType<Entity>(new EntityCodec());
            protocol.RegisterCodecForType<EntityInternal>(new EntityCodec());
            protocol.RegisterInheritanceCodecForType<GroupComponent>(new GroupComponentCodec());
            protocol.RegisterCodecForType<ComponentChangeCommand>(new ComponentChangeCommandCodec());
        }

        public void RegisterCommand<T>(CommandCode code) where T: Command
        {
            this.typeByCode.Add(code, typeof(T));
            this.codeByType.Add(typeof(T), code);
        }

        [Inject]
        public static ProtocolFlowInstancesCache ProtocolFlowInstances { get; set; }
    }
}

