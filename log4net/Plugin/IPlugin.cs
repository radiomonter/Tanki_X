namespace log4net.Plugin
{
    using log4net.Repository;
    using System;

    public interface IPlugin
    {
        void Attach(ILoggerRepository repository);
        void Shutdown();

        string Name { get; }
    }
}

