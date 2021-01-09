namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;

    public class ClientProtocolActivator : DefaultActivator<AutoCompleting>
    {
        protected override void Activate()
        {
            ServiceRegistry.Current.RegisterService<Protocol>(new ProtocolImpl());
        }
    }
}

