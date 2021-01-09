namespace Platform.System.Data.Statics.ClientConfigurator.API
{
    using System;

    public class ConfigWasNotFoundException : Exception
    {
        public ConfigWasNotFoundException(string configPath) : base("configPath: " + configPath)
        {
        }
    }
}

