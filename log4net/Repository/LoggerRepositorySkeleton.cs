namespace log4net.Repository
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.ObjectRenderer;
    using log4net.Plugin;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class LoggerRepositorySkeleton : ILoggerRepository
    {
        private string m_name;
        private log4net.ObjectRenderer.RendererMap m_rendererMap;
        private log4net.Plugin.PluginMap m_pluginMap;
        private log4net.Core.LevelMap m_levelMap;
        private Level m_threshold;
        private bool m_configured;
        private ICollection m_configurationMessages;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LoggerRepositoryShutdownEventHandler m_shutdownEvent;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LoggerRepositoryConfigurationResetEventHandler m_configurationResetEvent;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LoggerRepositoryConfigurationChangedEventHandler m_configurationChangedEvent;
        private PropertiesDictionary m_properties;
        private static readonly Type declaringType = typeof(LoggerRepositorySkeleton);

        public event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged
        {
            add
            {
                this.m_configurationChangedEvent += value;
            }
            remove
            {
                this.m_configurationChangedEvent -= value;
            }
        }

        public event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset
        {
            add
            {
                this.m_configurationResetEvent += value;
            }
            remove
            {
                this.m_configurationResetEvent -= value;
            }
        }

        private event LoggerRepositoryConfigurationChangedEventHandler m_configurationChangedEvent
        {
            add
            {
                LoggerRepositoryConfigurationChangedEventHandler configurationChangedEvent = this.m_configurationChangedEvent;
                while (true)
                {
                    LoggerRepositoryConfigurationChangedEventHandler objB = configurationChangedEvent;
                    configurationChangedEvent = Interlocked.CompareExchange<LoggerRepositoryConfigurationChangedEventHandler>(ref this.m_configurationChangedEvent, objB + value, configurationChangedEvent);
                    if (ReferenceEquals(configurationChangedEvent, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                LoggerRepositoryConfigurationChangedEventHandler configurationChangedEvent = this.m_configurationChangedEvent;
                while (true)
                {
                    LoggerRepositoryConfigurationChangedEventHandler objB = configurationChangedEvent;
                    configurationChangedEvent = Interlocked.CompareExchange<LoggerRepositoryConfigurationChangedEventHandler>(ref this.m_configurationChangedEvent, objB - value, configurationChangedEvent);
                    if (ReferenceEquals(configurationChangedEvent, objB))
                    {
                        return;
                    }
                }
            }
        }

        private event LoggerRepositoryConfigurationResetEventHandler m_configurationResetEvent
        {
            add
            {
                LoggerRepositoryConfigurationResetEventHandler configurationResetEvent = this.m_configurationResetEvent;
                while (true)
                {
                    LoggerRepositoryConfigurationResetEventHandler objB = configurationResetEvent;
                    configurationResetEvent = Interlocked.CompareExchange<LoggerRepositoryConfigurationResetEventHandler>(ref this.m_configurationResetEvent, objB + value, configurationResetEvent);
                    if (ReferenceEquals(configurationResetEvent, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                LoggerRepositoryConfigurationResetEventHandler configurationResetEvent = this.m_configurationResetEvent;
                while (true)
                {
                    LoggerRepositoryConfigurationResetEventHandler objB = configurationResetEvent;
                    configurationResetEvent = Interlocked.CompareExchange<LoggerRepositoryConfigurationResetEventHandler>(ref this.m_configurationResetEvent, objB - value, configurationResetEvent);
                    if (ReferenceEquals(configurationResetEvent, objB))
                    {
                        return;
                    }
                }
            }
        }

        private event LoggerRepositoryShutdownEventHandler m_shutdownEvent
        {
            add
            {
                LoggerRepositoryShutdownEventHandler shutdownEvent = this.m_shutdownEvent;
                while (true)
                {
                    LoggerRepositoryShutdownEventHandler objB = shutdownEvent;
                    shutdownEvent = Interlocked.CompareExchange<LoggerRepositoryShutdownEventHandler>(ref this.m_shutdownEvent, objB + value, shutdownEvent);
                    if (ReferenceEquals(shutdownEvent, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                LoggerRepositoryShutdownEventHandler shutdownEvent = this.m_shutdownEvent;
                while (true)
                {
                    LoggerRepositoryShutdownEventHandler objB = shutdownEvent;
                    shutdownEvent = Interlocked.CompareExchange<LoggerRepositoryShutdownEventHandler>(ref this.m_shutdownEvent, objB - value, shutdownEvent);
                    if (ReferenceEquals(shutdownEvent, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event LoggerRepositoryShutdownEventHandler ShutdownEvent
        {
            add
            {
                this.m_shutdownEvent += value;
            }
            remove
            {
                this.m_shutdownEvent -= value;
            }
        }

        protected LoggerRepositorySkeleton() : this(new PropertiesDictionary())
        {
        }

        protected LoggerRepositorySkeleton(PropertiesDictionary properties)
        {
            this.m_properties = properties;
            this.m_rendererMap = new log4net.ObjectRenderer.RendererMap();
            this.m_pluginMap = new log4net.Plugin.PluginMap(this);
            this.m_levelMap = new log4net.Core.LevelMap();
            this.m_configurationMessages = EmptyCollection.Instance;
            this.m_configured = false;
            this.AddBuiltinLevels();
            this.m_threshold = Level.All;
        }

        private void AddBuiltinLevels()
        {
            this.m_levelMap.Add(Level.Off);
            this.m_levelMap.Add(Level.Emergency);
            this.m_levelMap.Add(Level.Fatal);
            this.m_levelMap.Add(Level.Alert);
            this.m_levelMap.Add(Level.Critical);
            this.m_levelMap.Add(Level.Severe);
            this.m_levelMap.Add(Level.Error);
            this.m_levelMap.Add(Level.Warn);
            this.m_levelMap.Add(Level.Notice);
            this.m_levelMap.Add(Level.Info);
            this.m_levelMap.Add(Level.Debug);
            this.m_levelMap.Add(Level.Fine);
            this.m_levelMap.Add(Level.Trace);
            this.m_levelMap.Add(Level.Finer);
            this.m_levelMap.Add(Level.Verbose);
            this.m_levelMap.Add(Level.Finest);
            this.m_levelMap.Add(Level.All);
        }

        public virtual void AddRenderer(Type typeToRender, IObjectRenderer rendererInstance)
        {
            if (typeToRender == null)
            {
                throw new ArgumentNullException("typeToRender");
            }
            if (rendererInstance == null)
            {
                throw new ArgumentNullException("rendererInstance");
            }
            this.m_rendererMap.Put(typeToRender, rendererInstance);
        }

        public abstract ILogger Exists(string name);
        public abstract IAppender[] GetAppenders();
        public abstract ILogger[] GetCurrentLoggers();
        public abstract ILogger GetLogger(string name);
        public abstract void Log(LoggingEvent logEvent);
        protected virtual void OnConfigurationChanged(EventArgs e)
        {
            e ??= EventArgs.Empty;
            LoggerRepositoryConfigurationChangedEventHandler configurationChangedEvent = this.m_configurationChangedEvent;
            if (configurationChangedEvent != null)
            {
                configurationChangedEvent(this, e);
            }
        }

        protected virtual void OnConfigurationReset(EventArgs e)
        {
            e ??= EventArgs.Empty;
            LoggerRepositoryConfigurationResetEventHandler configurationResetEvent = this.m_configurationResetEvent;
            if (configurationResetEvent != null)
            {
                configurationResetEvent(this, e);
            }
        }

        protected virtual void OnShutdown(EventArgs e)
        {
            e ??= EventArgs.Empty;
            LoggerRepositoryShutdownEventHandler shutdownEvent = this.m_shutdownEvent;
            if (shutdownEvent != null)
            {
                shutdownEvent(this, e);
            }
        }

        public void RaiseConfigurationChanged(EventArgs e)
        {
            this.OnConfigurationChanged(e);
        }

        public virtual void ResetConfiguration()
        {
            this.m_rendererMap.Clear();
            this.m_levelMap.Clear();
            this.m_configurationMessages = EmptyCollection.Instance;
            this.AddBuiltinLevels();
            this.Configured = false;
            this.OnConfigurationReset(null);
        }

        public virtual void Shutdown()
        {
            PluginCollection.IPluginCollectionEnumerator enumerator = this.PluginMap.AllPlugins.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Shutdown();
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            this.OnShutdown(null);
        }

        public virtual string Name
        {
            get => 
                this.m_name;
            set => 
                this.m_name = value;
        }

        public virtual Level Threshold
        {
            get => 
                this.m_threshold;
            set
            {
                if (value != null)
                {
                    this.m_threshold = value;
                }
                else
                {
                    LogLog.Warn(declaringType, "LoggerRepositorySkeleton: Threshold cannot be set to null. Setting to ALL");
                    this.m_threshold = Level.All;
                }
            }
        }

        public virtual log4net.ObjectRenderer.RendererMap RendererMap =>
            this.m_rendererMap;

        public virtual log4net.Plugin.PluginMap PluginMap =>
            this.m_pluginMap;

        public virtual log4net.Core.LevelMap LevelMap =>
            this.m_levelMap;

        public virtual bool Configured
        {
            get => 
                this.m_configured;
            set => 
                this.m_configured = value;
        }

        public virtual ICollection ConfigurationMessages
        {
            get => 
                this.m_configurationMessages;
            set => 
                this.m_configurationMessages = value;
        }

        public PropertiesDictionary Properties =>
            this.m_properties;
    }
}

