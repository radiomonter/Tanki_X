namespace log4net.Repository
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.ObjectRenderer;
    using log4net.Plugin;
    using log4net.Util;
    using System;
    using System.Collections;

    public interface ILoggerRepository
    {
        event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged;

        event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset;

        event LoggerRepositoryShutdownEventHandler ShutdownEvent;

        ILogger Exists(string name);
        IAppender[] GetAppenders();
        ILogger[] GetCurrentLoggers();
        ILogger GetLogger(string name);
        void Log(LoggingEvent logEvent);
        void ResetConfiguration();
        void Shutdown();

        string Name { get; set; }

        log4net.ObjectRenderer.RendererMap RendererMap { get; }

        log4net.Plugin.PluginMap PluginMap { get; }

        log4net.Core.LevelMap LevelMap { get; }

        Level Threshold { get; set; }

        bool Configured { get; set; }

        ICollection ConfigurationMessages { get; set; }

        PropertiesDictionary Properties { get; }
    }
}

