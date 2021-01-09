namespace log4net.Repository
{
    using System;
    using System.Collections;

    public class ConfigurationChangedEventArgs : EventArgs
    {
        private readonly ICollection configurationMessages;

        public ConfigurationChangedEventArgs(ICollection configurationMessages)
        {
            this.configurationMessages = configurationMessages;
        }

        public ICollection ConfigurationMessages =>
            this.configurationMessages;
    }
}

