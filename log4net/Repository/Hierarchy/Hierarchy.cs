namespace log4net.Repository.Hierarchy
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml;

    public class Hierarchy : LoggerRepositorySkeleton, IBasicRepositoryConfigurator, IXmlRepositoryConfigurator
    {
        private ILoggerFactory m_defaultFactory;
        private Hashtable m_ht;
        private Logger m_root;
        private bool m_emittedNoAppenderWarning;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LoggerCreationEventHandler m_loggerCreatedEvent;
        private static readonly Type declaringType = typeof(log4net.Repository.Hierarchy.Hierarchy);

        public event LoggerCreationEventHandler LoggerCreatedEvent
        {
            add
            {
                this.m_loggerCreatedEvent += value;
            }
            remove
            {
                this.m_loggerCreatedEvent -= value;
            }
        }

        private event LoggerCreationEventHandler m_loggerCreatedEvent
        {
            add
            {
                LoggerCreationEventHandler loggerCreatedEvent = this.m_loggerCreatedEvent;
                while (true)
                {
                    LoggerCreationEventHandler objB = loggerCreatedEvent;
                    loggerCreatedEvent = Interlocked.CompareExchange<LoggerCreationEventHandler>(ref this.m_loggerCreatedEvent, objB + value, loggerCreatedEvent);
                    if (ReferenceEquals(loggerCreatedEvent, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                LoggerCreationEventHandler loggerCreatedEvent = this.m_loggerCreatedEvent;
                while (true)
                {
                    LoggerCreationEventHandler objB = loggerCreatedEvent;
                    loggerCreatedEvent = Interlocked.CompareExchange<LoggerCreationEventHandler>(ref this.m_loggerCreatedEvent, objB - value, loggerCreatedEvent);
                    if (ReferenceEquals(loggerCreatedEvent, objB))
                    {
                        return;
                    }
                }
            }
        }

        public Hierarchy() : this(new DefaultLoggerFactory())
        {
        }

        public Hierarchy(ILoggerFactory loggerFactory) : this(new PropertiesDictionary(), loggerFactory)
        {
        }

        public Hierarchy(PropertiesDictionary properties) : this(properties, new DefaultLoggerFactory())
        {
        }

        public Hierarchy(PropertiesDictionary properties, ILoggerFactory loggerFactory) : base(properties)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }
            this.m_defaultFactory = loggerFactory;
            this.m_ht = Hashtable.Synchronized(new Hashtable());
        }

        internal void AddLevel(LevelEntry levelEntry)
        {
            if (levelEntry == null)
            {
                throw new ArgumentNullException("levelEntry");
            }
            if (levelEntry.Name == null)
            {
                throw new ArgumentNullException("levelEntry.Name");
            }
            if (levelEntry.Value == -1)
            {
                Level level = this.LevelMap[levelEntry.Name];
                if (level == null)
                {
                    throw new InvalidOperationException("Cannot redefine level [" + levelEntry.Name + "] because it is not defined in the LevelMap. To define the level supply the level value.");
                }
                levelEntry.Value = level.Value;
            }
            this.LevelMap.Add(levelEntry.Name, levelEntry.Value, levelEntry.DisplayName);
        }

        internal void AddProperty(PropertyEntry propertyEntry)
        {
            if (propertyEntry == null)
            {
                throw new ArgumentNullException("propertyEntry");
            }
            if (propertyEntry.Key == null)
            {
                throw new ArgumentNullException("propertyEntry.Key");
            }
            base.Properties[propertyEntry.Key] = propertyEntry.Value;
        }

        protected void BasicRepositoryConfigure(params IAppender[] appenders)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                foreach (IAppender appender in appenders)
                {
                    this.Root.AddAppender(appender);
                }
            }
            this.Configured = true;
            this.ConfigurationMessages = items;
            this.OnConfigurationChanged(new ConfigurationChangedEventArgs(items));
        }

        public void Clear()
        {
            this.m_ht.Clear();
        }

        private static void CollectAppender(ArrayList appenderList, IAppender appender)
        {
            if (!appenderList.Contains(appender))
            {
                appenderList.Add(appender);
                IAppenderAttachable container = appender as IAppenderAttachable;
                if (container != null)
                {
                    CollectAppenders(appenderList, container);
                }
            }
        }

        private static void CollectAppenders(ArrayList appenderList, IAppenderAttachable container)
        {
            AppenderCollection.IAppenderCollectionEnumerator enumerator = container.Appenders.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    IAppender current = enumerator.Current;
                    CollectAppender(appenderList, current);
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
        }

        public override ILogger Exists(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            return (this.m_ht[new LoggerKey(name)] as Logger);
        }

        public override IAppender[] GetAppenders()
        {
            ArrayList appenderList = new ArrayList();
            CollectAppenders(appenderList, this.Root);
            foreach (Logger logger in this.GetCurrentLoggers())
            {
                CollectAppenders(appenderList, logger);
            }
            return (IAppender[]) appenderList.ToArray(typeof(IAppender));
        }

        public override ILogger[] GetCurrentLoggers()
        {
            ArrayList list = new ArrayList(this.m_ht.Count);
            IEnumerator enumerator = this.m_ht.Values.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (current is Logger)
                    {
                        list.Add(current);
                    }
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
            return (Logger[]) list.ToArray(typeof(Logger));
        }

        public override ILogger GetLogger(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            return this.GetLogger(name, this.m_defaultFactory);
        }

        public Logger GetLogger(string name, ILoggerFactory factory)
        {
            Logger logger2;
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            LoggerKey key = new LoggerKey(name);
            lock (this.m_ht)
            {
                Logger log = null;
                object obj3 = this.m_ht[key];
                if (obj3 == null)
                {
                    log = factory.CreateLogger(this, name);
                    log.Hierarchy = this;
                    this.m_ht[key] = log;
                    this.UpdateParents(log);
                    this.OnLoggerCreationEvent(log);
                    logger2 = log;
                }
                else
                {
                    Logger logger3 = obj3 as Logger;
                    if (logger3 != null)
                    {
                        logger2 = logger3;
                    }
                    else
                    {
                        ProvisionNode pn = obj3 as ProvisionNode;
                        if (pn == null)
                        {
                            logger2 = null;
                        }
                        else
                        {
                            log = factory.CreateLogger(this, name);
                            log.Hierarchy = this;
                            this.m_ht[key] = log;
                            UpdateChildren(pn, log);
                            this.UpdateParents(log);
                            this.OnLoggerCreationEvent(log);
                            logger2 = log;
                        }
                    }
                }
            }
            return logger2;
        }

        public bool IsDisabled(Level level)
        {
            if (level == null)
            {
                throw new ArgumentNullException("level");
            }
            return (!this.Configured || (this.Threshold > level));
        }

        public override void Log(LoggingEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException("logEvent");
            }
            this.GetLogger(logEvent.LoggerName, this.m_defaultFactory).Log(logEvent);
        }

        void IBasicRepositoryConfigurator.Configure(IAppender appender)
        {
            IAppender[] appenders = new IAppender[] { appender };
            this.BasicRepositoryConfigure(appenders);
        }

        void IBasicRepositoryConfigurator.Configure(params IAppender[] appenders)
        {
            this.BasicRepositoryConfigure(appenders);
        }

        void IXmlRepositoryConfigurator.Configure(XmlElement element)
        {
            this.XmlRepositoryConfigure(element);
        }

        protected virtual void OnLoggerCreationEvent(Logger logger)
        {
            LoggerCreationEventHandler loggerCreatedEvent = this.m_loggerCreatedEvent;
            if (loggerCreatedEvent != null)
            {
                loggerCreatedEvent(this, new LoggerCreationEventArgs(logger));
            }
        }

        public override void ResetConfiguration()
        {
            this.Root.Level = this.LevelMap.LookupWithDefault(Level.Debug);
            this.Threshold = this.LevelMap.LookupWithDefault(Level.All);
            lock (this.m_ht)
            {
                this.Shutdown();
                foreach (Logger logger in this.GetCurrentLoggers())
                {
                    logger.Level = null;
                    logger.Additivity = true;
                }
            }
            base.ResetConfiguration();
            this.OnConfigurationChanged(null);
        }

        public override void Shutdown()
        {
            LogLog.Debug(declaringType, "Shutdown called on Hierarchy [" + this.Name + "]");
            this.Root.CloseNestedAppenders();
            lock (this.m_ht)
            {
                ILogger[] currentLoggers = this.GetCurrentLoggers();
                ILogger[] loggerArray2 = currentLoggers;
                int index = 0;
                while (true)
                {
                    if (index >= loggerArray2.Length)
                    {
                        this.Root.RemoveAllAppenders();
                        foreach (Logger logger2 in currentLoggers)
                        {
                            logger2.RemoveAllAppenders();
                        }
                        break;
                    }
                    ((Logger) loggerArray2[index]).CloseNestedAppenders();
                    index++;
                }
            }
            base.Shutdown();
        }

        private static void UpdateChildren(ProvisionNode pn, Logger log)
        {
            for (int i = 0; i < pn.Count; i++)
            {
                Logger logger = (Logger) pn[i];
                if (!logger.Parent.Name.StartsWith(log.Name))
                {
                    log.Parent = logger.Parent;
                    logger.Parent = log;
                }
            }
        }

        private void UpdateParents(Logger log)
        {
            string name = log.Name;
            bool flag = false;
            for (int i = name.LastIndexOf('.', name.Length - 1); i >= 0; i = name.LastIndexOf('.', i - 1))
            {
                string str2 = name.Substring(0, i);
                LoggerKey key = new LoggerKey(str2);
                object obj2 = this.m_ht[key];
                if (obj2 == null)
                {
                    this.m_ht[key] = new ProvisionNode(log);
                }
                else
                {
                    Logger logger = obj2 as Logger;
                    if (logger != null)
                    {
                        flag = true;
                        log.Parent = logger;
                        break;
                    }
                    ProvisionNode node2 = obj2 as ProvisionNode;
                    if (node2 != null)
                    {
                        node2.Add(log);
                    }
                    else
                    {
                        LogLog.Error(declaringType, "Unexpected object type [" + obj2.GetType() + "] in ht.", new LogException());
                    }
                }
                if (i == 0)
                {
                    break;
                }
            }
            if (!flag)
            {
                log.Parent = this.Root;
            }
        }

        protected void XmlRepositoryConfigure(XmlElement element)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                new XmlHierarchyConfigurator(this).Configure(element);
            }
            this.Configured = true;
            this.ConfigurationMessages = items;
            this.OnConfigurationChanged(new ConfigurationChangedEventArgs(items));
        }

        public bool EmittedNoAppenderWarning
        {
            get => 
                this.m_emittedNoAppenderWarning;
            set => 
                this.m_emittedNoAppenderWarning = value;
        }

        public Logger Root
        {
            get
            {
                if (this.m_root == null)
                {
                    lock (this)
                    {
                        if (this.m_root == null)
                        {
                            Logger logger = this.m_defaultFactory.CreateLogger(this, null);
                            logger.Hierarchy = this;
                            this.m_root = logger;
                        }
                    }
                }
                return this.m_root;
            }
        }

        public ILoggerFactory LoggerFactory
        {
            get => 
                this.m_defaultFactory;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.m_defaultFactory = value;
            }
        }

        internal class LevelEntry
        {
            private int m_levelValue = -1;
            private string m_levelName;
            private string m_levelDisplayName;

            public override string ToString()
            {
                object[] objArray1 = new object[] { "LevelEntry(Value=", this.m_levelValue, ", Name=", this.m_levelName, ", DisplayName=", this.m_levelDisplayName, ")" };
                return string.Concat(objArray1);
            }

            public int Value
            {
                get => 
                    this.m_levelValue;
                set => 
                    this.m_levelValue = value;
            }

            public string Name
            {
                get => 
                    this.m_levelName;
                set => 
                    this.m_levelName = value;
            }

            public string DisplayName
            {
                get => 
                    this.m_levelDisplayName;
                set => 
                    this.m_levelDisplayName = value;
            }
        }
    }
}

