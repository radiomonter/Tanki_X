namespace Platform.System.Data.Statics.ClientConfigurator.Impl
{
    using System;

    public interface ConfigurationProfile
    {
        bool Match(string configName);
    }
}

