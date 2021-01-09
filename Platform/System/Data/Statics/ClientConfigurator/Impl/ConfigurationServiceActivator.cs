namespace Platform.System.Data.Statics.ClientConfigurator.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;

    public class ConfigurationServiceActivator : DefaultActivator<AutoCompleting>
    {
        protected override void Activate()
        {
            ServiceRegistry.Current.RegisterService<ConfigurationService>(new ConfigurationServiceImpl());
        }
    }
}

