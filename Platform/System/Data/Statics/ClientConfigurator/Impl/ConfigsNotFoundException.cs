namespace Platform.System.Data.Statics.ClientConfigurator.Impl
{
    using System;

    public class ConfigsNotFoundException : Exception
    {
        public ConfigsNotFoundException(string message) : base(message)
        {
        }
    }
}

