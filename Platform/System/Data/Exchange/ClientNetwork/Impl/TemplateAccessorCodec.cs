namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientProtocol.Impl;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TemplateAccessorCodec : Codec
    {
        private readonly TemplateRegistry templateRegistry;
        private Codec longCodec;
        private Codec stringCodec;

        public TemplateAccessorCodec(TemplateRegistry templateRegistry)
        {
            this.templateRegistry = templateRegistry;
        }

        public object Decode(ProtocolBuffer protocolBuffer)
        {
            object obj2;
            long id = (long) this.longCodec.Decode(protocolBuffer);
            TemplateDescription templateInfo = this.templateRegistry.GetTemplateInfo(id);
            string configPath = (string) this.stringCodec.Decode(protocolBuffer);
            try
            {
                obj2 = new TemplateAccessor(templateInfo, configPath);
            }
            catch (Exception exception)
            {
                throw new Exception("templateType = " + templateInfo, exception);
            }
            return obj2;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            throw new NotImplementedException();
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            throw new NotImplementedException();
        }

        public void Init(Protocol protocol)
        {
            this.longCodec = protocol.GetCodec(typeof(long));
            this.stringCodec = new OptionalCodec((NotOptionalCodec) protocol.GetCodec(typeof(string)));
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }
    }
}

