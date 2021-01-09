namespace log4net.Core
{
    using log4net;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.Permissions;
    using System.Threading;

    [Serializable]
    public class LoggingEvent : ISerializable
    {
        private static readonly Type declaringType = typeof(LoggingEvent);
        private LoggingEventData m_data;
        private CompositeProperties m_compositeProperties;
        private PropertiesDictionary m_eventProperties;
        private readonly Type m_callerStackBoundaryDeclaringType;
        private readonly object m_message;
        private readonly Exception m_thrownException;
        private ILoggerRepository m_repository;
        private FixFlags m_fixFlags;
        private bool m_cacheUpdatable;
        public const string HostNameProperty = "log4net:HostName";
        public const string IdentityProperty = "log4net:Identity";
        public const string UserNameProperty = "log4net:UserName";

        public LoggingEvent(LoggingEventData data) : this(null, null, data)
        {
        }

        protected LoggingEvent(SerializationInfo info, StreamingContext context)
        {
            this.m_cacheUpdatable = true;
            this.m_data.LoggerName = info.GetString("LoggerName");
            this.m_data.Level = (log4net.Core.Level) info.GetValue("Level", typeof(log4net.Core.Level));
            this.m_data.Message = info.GetString("Message");
            this.m_data.ThreadName = info.GetString("ThreadName");
            this.m_data.TimeStamp = info.GetDateTime("TimeStamp");
            this.m_data.LocationInfo = (LocationInfo) info.GetValue("LocationInfo", typeof(LocationInfo));
            this.m_data.UserName = info.GetString("UserName");
            this.m_data.ExceptionString = info.GetString("ExceptionString");
            this.m_data.Properties = (PropertiesDictionary) info.GetValue("Properties", typeof(PropertiesDictionary));
            this.m_data.Domain = info.GetString("Domain");
            this.m_data.Identity = info.GetString("Identity");
            this.m_fixFlags = FixFlags.All;
        }

        public LoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, LoggingEventData data) : this(callerStackBoundaryDeclaringType, repository, data, FixFlags.All)
        {
        }

        public LoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, LoggingEventData data, FixFlags fixedData)
        {
            this.m_cacheUpdatable = true;
            this.m_callerStackBoundaryDeclaringType = callerStackBoundaryDeclaringType;
            this.m_repository = repository;
            this.m_data = data;
            this.m_fixFlags = fixedData;
        }

        public LoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, string loggerName, log4net.Core.Level level, object message, Exception exception)
        {
            this.m_cacheUpdatable = true;
            this.m_callerStackBoundaryDeclaringType = callerStackBoundaryDeclaringType;
            this.m_message = message;
            this.m_repository = repository;
            this.m_thrownException = exception;
            this.m_data.LoggerName = loggerName;
            this.m_data.Level = level;
            this.m_data.TimeStamp = DateTime.Now;
        }

        private void CacheProperties()
        {
            if ((this.m_data.Properties == null) && this.m_cacheUpdatable)
            {
                if (this.m_compositeProperties == null)
                {
                    this.CreateCompositeProperties();
                }
                PropertiesDictionary dictionary2 = new PropertiesDictionary();
                IEnumerator enumerator = ((IEnumerable) this.m_compositeProperties.Flatten()).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                        string key = current.Key as string;
                        if (key != null)
                        {
                            object fixedObject = current.Value;
                            IFixingRequired required = fixedObject as IFixingRequired;
                            if (required != null)
                            {
                                fixedObject = required.GetFixedObject();
                            }
                            if (fixedObject != null)
                            {
                                dictionary2[key] = fixedObject;
                            }
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
                this.m_data.Properties = dictionary2;
            }
        }

        private void CreateCompositeProperties()
        {
            this.m_compositeProperties = new CompositeProperties();
            if (this.m_eventProperties != null)
            {
                this.m_compositeProperties.Add(this.m_eventProperties);
            }
            PropertiesDictionary properties = ThreadContext.Properties.GetProperties(false);
            if (properties != null)
            {
                this.m_compositeProperties.Add(properties);
            }
            PropertiesDictionary dictionary2 = new PropertiesDictionary {
                ["log4net:UserName"] = this.UserName,
                ["log4net:Identity"] = this.Identity
            };
            this.m_compositeProperties.Add(dictionary2);
            this.m_compositeProperties.Add(GlobalContext.Properties.GetReadOnlyProperties());
        }

        internal void EnsureRepository(ILoggerRepository repository)
        {
            if (repository != null)
            {
                this.m_repository = repository;
            }
        }

        [Obsolete("Use Fix property")]
        public void FixVolatileData()
        {
            this.Fix = FixFlags.All;
        }

        protected void FixVolatileData(FixFlags flags)
        {
            object renderedMessage = null;
            this.m_cacheUpdatable = true;
            FixFlags flags2 = (flags ^ this.m_fixFlags) & flags;
            if (flags2 > FixFlags.None)
            {
                if ((flags2 & FixFlags.Message) != FixFlags.None)
                {
                    renderedMessage = this.RenderedMessage;
                    this.m_fixFlags |= FixFlags.Message;
                }
                if ((flags2 & FixFlags.ThreadName) != FixFlags.None)
                {
                    renderedMessage = this.ThreadName;
                    this.m_fixFlags |= FixFlags.ThreadName;
                }
                if ((flags2 & FixFlags.LocationInfo) != FixFlags.None)
                {
                    renderedMessage = this.LocationInformation;
                    this.m_fixFlags |= FixFlags.LocationInfo;
                }
                if ((flags2 & FixFlags.UserName) != FixFlags.None)
                {
                    renderedMessage = this.UserName;
                    this.m_fixFlags |= FixFlags.UserName;
                }
                if ((flags2 & FixFlags.Domain) != FixFlags.None)
                {
                    renderedMessage = this.Domain;
                    this.m_fixFlags |= FixFlags.Domain;
                }
                if ((flags2 & FixFlags.Identity) != FixFlags.None)
                {
                    renderedMessage = this.Identity;
                    this.m_fixFlags |= FixFlags.Identity;
                }
                if ((flags2 & FixFlags.Exception) != FixFlags.None)
                {
                    renderedMessage = this.GetExceptionString();
                    this.m_fixFlags |= FixFlags.Exception;
                }
                if ((flags2 & FixFlags.Properties) != FixFlags.None)
                {
                    this.CacheProperties();
                    this.m_fixFlags |= FixFlags.Properties;
                }
            }
            if (renderedMessage != null)
            {
            }
            this.m_cacheUpdatable = false;
        }

        [Obsolete("Use Fix property")]
        public void FixVolatileData(bool fastButLoose)
        {
            this.Fix = !fastButLoose ? FixFlags.All : FixFlags.Partial;
        }

        public string GetExceptionString()
        {
            if ((this.m_data.ExceptionString == null) && this.m_cacheUpdatable)
            {
                this.m_data.ExceptionString = (this.m_thrownException == null) ? string.Empty : ((this.m_repository == null) ? this.m_thrownException.ToString() : this.m_repository.RendererMap.FindAndRender(this.m_thrownException));
            }
            return this.m_data.ExceptionString;
        }

        [Obsolete("Use GetExceptionString instead")]
        public string GetExceptionStrRep() => 
            this.GetExceptionString();

        public LoggingEventData GetLoggingEventData() => 
            this.GetLoggingEventData(FixFlags.Partial);

        public LoggingEventData GetLoggingEventData(FixFlags fixFlags)
        {
            this.Fix = fixFlags;
            return this.m_data;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LoggerName", this.m_data.LoggerName);
            info.AddValue("Level", this.m_data.Level);
            info.AddValue("Message", this.m_data.Message);
            info.AddValue("ThreadName", this.m_data.ThreadName);
            info.AddValue("TimeStamp", this.m_data.TimeStamp);
            info.AddValue("LocationInfo", this.m_data.LocationInfo);
            info.AddValue("UserName", this.m_data.UserName);
            info.AddValue("ExceptionString", this.m_data.ExceptionString);
            info.AddValue("Properties", this.m_data.Properties);
            info.AddValue("Domain", this.m_data.Domain);
            info.AddValue("Identity", this.m_data.Identity);
        }

        public PropertiesDictionary GetProperties()
        {
            if (this.m_data.Properties != null)
            {
                return this.m_data.Properties;
            }
            if (this.m_compositeProperties == null)
            {
                this.CreateCompositeProperties();
            }
            return this.m_compositeProperties.Flatten();
        }

        public object LookupProperty(string key)
        {
            if (this.m_data.Properties != null)
            {
                return this.m_data.Properties[key];
            }
            if (this.m_compositeProperties == null)
            {
                this.CreateCompositeProperties();
            }
            return this.m_compositeProperties[key];
        }

        public void WriteRenderedMessage(TextWriter writer)
        {
            if (this.m_data.Message != null)
            {
                writer.Write(this.m_data.Message);
            }
            else if (this.m_message != null)
            {
                if (this.m_message is string)
                {
                    writer.Write(this.m_message as string);
                }
                else if (this.m_repository != null)
                {
                    this.m_repository.RendererMap.FindAndRender(this.m_message, writer);
                }
                else
                {
                    writer.Write(this.m_message.ToString());
                }
            }
        }

        public static DateTime StartTime =>
            SystemInfo.ProcessStartTime;

        public log4net.Core.Level Level =>
            this.m_data.Level;

        public DateTime TimeStamp =>
            this.m_data.TimeStamp;

        public string LoggerName =>
            this.m_data.LoggerName;

        public LocationInfo LocationInformation
        {
            get
            {
                if ((this.m_data.LocationInfo == null) && this.m_cacheUpdatable)
                {
                    this.m_data.LocationInfo = new LocationInfo(this.m_callerStackBoundaryDeclaringType);
                }
                return this.m_data.LocationInfo;
            }
        }

        public object MessageObject =>
            this.m_message;

        public Exception ExceptionObject =>
            this.m_thrownException;

        public ILoggerRepository Repository =>
            this.m_repository;

        public string RenderedMessage
        {
            get
            {
                if ((this.m_data.Message == null) && this.m_cacheUpdatable)
                {
                    this.m_data.Message = (this.m_message != null) ? (!(this.m_message is string) ? ((this.m_repository == null) ? this.m_message.ToString() : this.m_repository.RendererMap.FindAndRender(this.m_message)) : (this.m_message as string)) : string.Empty;
                }
                return this.m_data.Message;
            }
        }

        public string ThreadName
        {
            get
            {
                if ((this.m_data.ThreadName == null) && this.m_cacheUpdatable)
                {
                    this.m_data.ThreadName = Thread.CurrentThread.Name;
                    if ((this.m_data.ThreadName == null) || (this.m_data.ThreadName.Length == 0))
                    {
                        try
                        {
                            this.m_data.ThreadName = SystemInfo.CurrentThreadId.ToString(NumberFormatInfo.InvariantInfo);
                        }
                        catch (SecurityException)
                        {
                            LogLog.Debug(declaringType, "Security exception while trying to get current thread ID. Error Ignored. Empty thread name.");
                            this.m_data.ThreadName = Thread.CurrentThread.GetHashCode().ToString(CultureInfo.InvariantCulture);
                        }
                    }
                }
                return this.m_data.ThreadName;
            }
        }

        public string UserName
        {
            get
            {
                if ((this.m_data.UserName == null) && this.m_cacheUpdatable)
                {
                    this.m_data.UserName = SystemInfo.NotAvailableText;
                }
                return this.m_data.UserName;
            }
        }

        public string Identity
        {
            get
            {
                if ((this.m_data.Identity == null) && this.m_cacheUpdatable)
                {
                    this.m_data.Identity = SystemInfo.NotAvailableText;
                }
                return this.m_data.Identity;
            }
        }

        public string Domain
        {
            get
            {
                if ((this.m_data.Domain == null) && this.m_cacheUpdatable)
                {
                    this.m_data.Domain = SystemInfo.ApplicationFriendlyName;
                }
                return this.m_data.Domain;
            }
        }

        public PropertiesDictionary Properties
        {
            get
            {
                if (this.m_data.Properties != null)
                {
                    return this.m_data.Properties;
                }
                this.m_eventProperties ??= new PropertiesDictionary();
                return this.m_eventProperties;
            }
        }

        public FixFlags Fix
        {
            get => 
                this.m_fixFlags;
            set => 
                this.FixVolatileData(value);
        }
    }
}

