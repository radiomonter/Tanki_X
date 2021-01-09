namespace Platform.Library.ClientLogger.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using System;

    public class ClientLoggerActivator : DefaultActivator<AutoCompleting>
    {
        protected override void Activate()
        {
            LoggerProvider.Init();
        }
    }
}

