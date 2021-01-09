namespace log4net.Core
{
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class CompactRepositorySelector : IRepositorySelector
    {
        private const string DefaultRepositoryName = "log4net-default-repository";
        private readonly Hashtable m_name2repositoryMap = new Hashtable();
        private readonly Type m_defaultRepositoryType;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent;
        private static readonly Type declaringType = typeof(CompactRepositorySelector);

        public event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent
        {
            add
            {
                this.m_loggerRepositoryCreatedEvent += value;
            }
            remove
            {
                this.m_loggerRepositoryCreatedEvent -= value;
            }
        }

        private event LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent
        {
            add
            {
                LoggerRepositoryCreationEventHandler loggerRepositoryCreatedEvent = this.m_loggerRepositoryCreatedEvent;
                while (true)
                {
                    LoggerRepositoryCreationEventHandler objB = loggerRepositoryCreatedEvent;
                    loggerRepositoryCreatedEvent = Interlocked.CompareExchange<LoggerRepositoryCreationEventHandler>(ref this.m_loggerRepositoryCreatedEvent, objB + value, loggerRepositoryCreatedEvent);
                    if (ReferenceEquals(loggerRepositoryCreatedEvent, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                LoggerRepositoryCreationEventHandler loggerRepositoryCreatedEvent = this.m_loggerRepositoryCreatedEvent;
                while (true)
                {
                    LoggerRepositoryCreationEventHandler objB = loggerRepositoryCreatedEvent;
                    loggerRepositoryCreatedEvent = Interlocked.CompareExchange<LoggerRepositoryCreationEventHandler>(ref this.m_loggerRepositoryCreatedEvent, objB - value, loggerRepositoryCreatedEvent);
                    if (ReferenceEquals(loggerRepositoryCreatedEvent, objB))
                    {
                        return;
                    }
                }
            }
        }

        public CompactRepositorySelector(Type defaultRepositoryType)
        {
            if (defaultRepositoryType == null)
            {
                throw new ArgumentNullException("defaultRepositoryType");
            }
            if (!typeof(ILoggerRepository).IsAssignableFrom(defaultRepositoryType))
            {
                throw SystemInfo.CreateArgumentOutOfRangeException("defaultRepositoryType", defaultRepositoryType, "Parameter: defaultRepositoryType, Value: [" + defaultRepositoryType + "] out of range. Argument must implement the ILoggerRepository interface");
            }
            this.m_defaultRepositoryType = defaultRepositoryType;
            LogLog.Debug(declaringType, "defaultRepositoryType [" + this.m_defaultRepositoryType + "]");
        }

        public ILoggerRepository CreateRepository(Assembly assembly, Type repositoryType)
        {
            repositoryType ??= this.m_defaultRepositoryType;
            lock (this)
            {
                ILoggerRepository repository = this.m_name2repositoryMap["log4net-default-repository"] as ILoggerRepository;
                return this.CreateRepository("log4net-default-repository", repositoryType);
            }
        }

        public ILoggerRepository CreateRepository(string repositoryName, Type repositoryType)
        {
            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }
            repositoryType ??= this.m_defaultRepositoryType;
            lock (this)
            {
                ILoggerRepository repository = null;
                if (this.m_name2repositoryMap[repositoryName] is ILoggerRepository)
                {
                    throw new LogException("Repository [" + repositoryName + "] is already defined. Repositories cannot be redefined.");
                }
                object[] objArray1 = new object[] { "Creating repository [", repositoryName, "] using type [", repositoryType, "]" };
                LogLog.Debug(declaringType, string.Concat(objArray1));
                repository = (ILoggerRepository) Activator.CreateInstance(repositoryType);
                repository.Name = repositoryName;
                this.m_name2repositoryMap[repositoryName] = repository;
                this.OnLoggerRepositoryCreatedEvent(repository);
                return repository;
            }
        }

        public bool ExistsRepository(string repositoryName)
        {
            lock (this)
            {
                return this.m_name2repositoryMap.ContainsKey(repositoryName);
            }
        }

        public ILoggerRepository[] GetAllRepositories()
        {
            lock (this)
            {
                ICollection values = this.m_name2repositoryMap.Values;
                ILoggerRepository[] array = new ILoggerRepository[values.Count];
                values.CopyTo(array, 0);
                return array;
            }
        }

        public ILoggerRepository GetRepository(Assembly assembly) => 
            this.CreateRepository(assembly, this.m_defaultRepositoryType);

        public ILoggerRepository GetRepository(string repositoryName)
        {
            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }
            lock (this)
            {
                ILoggerRepository repository = this.m_name2repositoryMap[repositoryName] as ILoggerRepository;
                if (repository == null)
                {
                    throw new LogException("Repository [" + repositoryName + "] is NOT defined.");
                }
                return repository;
            }
        }

        protected virtual void OnLoggerRepositoryCreatedEvent(ILoggerRepository repository)
        {
            LoggerRepositoryCreationEventHandler loggerRepositoryCreatedEvent = this.m_loggerRepositoryCreatedEvent;
            if (loggerRepositoryCreatedEvent != null)
            {
                loggerRepositoryCreatedEvent(this, new LoggerRepositoryCreationEventArgs(repository));
            }
        }
    }
}

