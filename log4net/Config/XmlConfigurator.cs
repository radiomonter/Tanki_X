namespace log4net.Config
{
    using log4net;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Xml;

    public sealed class XmlConfigurator
    {
        private static readonly Hashtable m_repositoryName2ConfigAndWatchHandler = new Hashtable();
        private static readonly Type declaringType = typeof(XmlConfigurator);

        private XmlConfigurator()
        {
        }

        public static ICollection Configure() => 
            Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));

        public static ICollection Configure(ILoggerRepository repository)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigure(repository);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection Configure(FileInfo configFile)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
            }
            return items;
        }

        public static ICollection Configure(Stream configStream)
        {
            ArrayList items = new ArrayList();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigure(repository, configStream);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection Configure(Uri configUri)
        {
            ArrayList items = new ArrayList();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigure(repository, configUri);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection Configure(XmlElement element)
        {
            ArrayList items = new ArrayList();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigureFromXml(repository, element);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection Configure(ILoggerRepository repository, FileInfo configFile)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigure(repository, configFile);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection Configure(ILoggerRepository repository, Stream configStream)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigure(repository, configStream);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection Configure(ILoggerRepository repository, Uri configUri)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigure(repository, configUri);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection Configure(ILoggerRepository repository, XmlElement element)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                LogLog.Debug(declaringType, "configuring repository [" + repository.Name + "] using XML element");
                InternalConfigureFromXml(repository, element);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection ConfigureAndWatch(FileInfo configFile)
        {
            ArrayList items = new ArrayList();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigureAndWatch(repository, configFile);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection ConfigureAndWatch(ILoggerRepository repository, FileInfo configFile)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigureAndWatch(repository, configFile);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        private static void InternalConfigure(ILoggerRepository repository)
        {
            LogLog.Debug(declaringType, "configuring repository [" + repository.Name + "] using .config file section");
            try
            {
                LogLog.Debug(declaringType, "Application config file is [" + SystemInfo.ConfigurationFileLocation + "]");
            }
            catch
            {
                LogLog.Debug(declaringType, "Application config file location unknown");
            }
            try
            {
                XmlElement config = ConfigurationSettings.GetConfig("log4net") as XmlElement;
                if (config == null)
                {
                    LogLog.Error(declaringType, "Failed to find configuration section 'log4net' in the application's .config file. Check your .config file for the <log4net> and <configSections> elements. The configuration section should look like: <section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,log4net\" />");
                }
                else
                {
                    InternalConfigureFromXml(repository, config);
                }
            }
            catch (ConfigurationException exception)
            {
                if (exception.BareMessage.IndexOf("Unrecognized element") >= 0)
                {
                    LogLog.Error(declaringType, "Failed to parse config file. Check your .config file is well formed XML.", exception);
                }
                else
                {
                    LogLog.Error(declaringType, "Failed to parse config file. Is the <configSections> specified as: " + ("<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler," + Assembly.GetExecutingAssembly().FullName + "\" />"), exception);
                }
            }
        }

        private static void InternalConfigure(ILoggerRepository repository, FileInfo configFile)
        {
            FileStream stream;
            object[] objArray1 = new object[] { "configuring repository [", repository.Name, "] using file [", configFile, "]" };
            LogLog.Debug(declaringType, string.Concat(objArray1));
            if (configFile != null)
            {
                if (!File.Exists(configFile.FullName))
                {
                    LogLog.Debug(declaringType, "config file [" + configFile.FullName + "] not found. Configuration unchanged.");
                    return;
                }
                else
                {
                    stream = null;
                    int num = 5;
                    while (--num >= 0)
                    {
                        try
                        {
                            stream = configFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                            break;
                        }
                        catch (IOException exception)
                        {
                            if (num == 0)
                            {
                                LogLog.Error(declaringType, "Failed to open XML config file [" + configFile.Name + "]", exception);
                                stream = null;
                            }
                            Thread.Sleep(250);
                        }
                    }
                }
            }
            else
            {
                LogLog.Error(declaringType, "Configure called with null 'configFile' parameter");
                return;
            }
            if (stream != null)
            {
                try
                {
                    InternalConfigure(repository, stream);
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        private static void InternalConfigure(ILoggerRepository repository, Stream configStream)
        {
            LogLog.Debug(declaringType, "configuring repository [" + repository.Name + "] using stream");
            if (configStream == null)
            {
                LogLog.Error(declaringType, "Configure called with null 'configStream' parameter");
            }
            else
            {
                XmlDocument document = new XmlDocument();
                try
                {
                    XmlReaderSettings settings = new XmlReaderSettings {
                        ProhibitDtd = false
                    };
                    document.Load(XmlReader.Create(configStream, settings));
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Error while loading XML configuration", exception);
                    document = null;
                }
                if (document != null)
                {
                    LogLog.Debug(declaringType, "loading XML configuration");
                    XmlNodeList elementsByTagName = document.GetElementsByTagName("log4net");
                    if (elementsByTagName.Count == 0)
                    {
                        LogLog.Debug(declaringType, "XML configuration does not contain a <log4net> element. Configuration Aborted.");
                    }
                    else if (elementsByTagName.Count > 1)
                    {
                        LogLog.Error(declaringType, "XML configuration contains [" + elementsByTagName.Count + "] <log4net> elements. Only one is allowed. Configuration Aborted.");
                    }
                    else
                    {
                        InternalConfigureFromXml(repository, elementsByTagName[0] as XmlElement);
                    }
                }
            }
        }

        private static void InternalConfigure(ILoggerRepository repository, Uri configUri)
        {
            object[] objArray1 = new object[] { "configuring repository [", repository.Name, "] using URI [", configUri, "]" };
            LogLog.Debug(declaringType, string.Concat(objArray1));
            if (configUri == null)
            {
                LogLog.Error(declaringType, "Configure called with null 'configUri' parameter");
            }
            else if (configUri.IsFile)
            {
                InternalConfigure(repository, new FileInfo(configUri.LocalPath));
            }
            else
            {
                WebRequest request = null;
                try
                {
                    request = WebRequest.Create(configUri);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to create WebRequest for URI [" + configUri + "]", exception);
                }
                if (request != null)
                {
                    try
                    {
                        request.Credentials = CredentialCache.DefaultCredentials;
                    }
                    catch
                    {
                    }
                    try
                    {
                        WebResponse response = request.GetResponse();
                        if (response != null)
                        {
                            try
                            {
                                using (Stream stream = response.GetResponseStream())
                                {
                                    InternalConfigure(repository, stream);
                                }
                            }
                            finally
                            {
                                response.Close();
                            }
                        }
                    }
                    catch (Exception exception2)
                    {
                        LogLog.Error(declaringType, "Failed to request config from URI [" + configUri + "]", exception2);
                    }
                }
            }
        }

        private static void InternalConfigureAndWatch(ILoggerRepository repository, FileInfo configFile)
        {
            object[] objArray1 = new object[] { "configuring repository [", repository.Name, "] using file [", configFile, "] watching for file updates" };
            LogLog.Debug(declaringType, string.Concat(objArray1));
            if (configFile == null)
            {
                LogLog.Error(declaringType, "ConfigureAndWatch called with null 'configFile' parameter");
            }
            else
            {
                InternalConfigure(repository, configFile);
                try
                {
                    lock (m_repositoryName2ConfigAndWatchHandler)
                    {
                        ConfigureAndWatchHandler handler = (ConfigureAndWatchHandler) m_repositoryName2ConfigAndWatchHandler[configFile.FullName];
                        if (handler != null)
                        {
                            m_repositoryName2ConfigAndWatchHandler.Remove(configFile.FullName);
                            handler.Dispose();
                        }
                        handler = new ConfigureAndWatchHandler(repository, configFile);
                        m_repositoryName2ConfigAndWatchHandler[configFile.FullName] = handler;
                    }
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to initialize configuration file watcher for file [" + configFile.FullName + "]", exception);
                }
            }
        }

        private static void InternalConfigureFromXml(ILoggerRepository repository, XmlElement element)
        {
            if (element == null)
            {
                LogLog.Error(declaringType, "ConfigureFromXml called with null 'element' parameter");
            }
            else if (repository == null)
            {
                LogLog.Error(declaringType, "ConfigureFromXml called with null 'repository' parameter");
            }
            else
            {
                LogLog.Debug(declaringType, "Configuring Repository [" + repository.Name + "]");
                IXmlRepositoryConfigurator configurator = repository as IXmlRepositoryConfigurator;
                if (configurator == null)
                {
                    LogLog.Warn(declaringType, "Repository [" + repository + "] does not support the XmlConfigurator");
                }
                else
                {
                    XmlDocument document = new XmlDocument();
                    configurator.Configure((XmlElement) document.AppendChild(document.ImportNode(element, true)));
                }
            }
        }

        private sealed class ConfigureAndWatchHandler : IDisposable
        {
            private FileInfo m_configFile;
            private ILoggerRepository m_repository;
            private Timer m_timer;
            private const int TimeoutMillis = 500;
            private FileSystemWatcher m_watcher;

            public ConfigureAndWatchHandler(ILoggerRepository repository, FileInfo configFile)
            {
                this.m_repository = repository;
                this.m_configFile = configFile;
                this.m_watcher = new FileSystemWatcher();
                this.m_watcher.Path = this.m_configFile.DirectoryName;
                this.m_watcher.Filter = this.m_configFile.Name;
                this.m_watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName;
                this.m_watcher.Changed += new FileSystemEventHandler(this.ConfigureAndWatchHandler_OnChanged);
                this.m_watcher.Created += new FileSystemEventHandler(this.ConfigureAndWatchHandler_OnChanged);
                this.m_watcher.Deleted += new FileSystemEventHandler(this.ConfigureAndWatchHandler_OnChanged);
                this.m_watcher.Renamed += new RenamedEventHandler(this.ConfigureAndWatchHandler_OnRenamed);
                this.m_watcher.EnableRaisingEvents = true;
                this.m_timer = new Timer(new TimerCallback(this.OnWatchedFileChange), null, -1, -1);
            }

            private void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
            {
                object[] objArray1 = new object[] { "ConfigureAndWatchHandler: ", e.ChangeType, " [", this.m_configFile.FullName, "]" };
                LogLog.Debug(XmlConfigurator.declaringType, string.Concat(objArray1));
                this.m_timer.Change(500, -1);
            }

            private void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e)
            {
                object[] objArray1 = new object[] { "ConfigureAndWatchHandler: ", e.ChangeType, " [", this.m_configFile.FullName, "]" };
                LogLog.Debug(XmlConfigurator.declaringType, string.Concat(objArray1));
                this.m_timer.Change(500, -1);
            }

            public void Dispose()
            {
                this.m_watcher.EnableRaisingEvents = false;
                this.m_watcher.Dispose();
                this.m_timer.Dispose();
            }

            private void OnWatchedFileChange(object state)
            {
                XmlConfigurator.InternalConfigure(this.m_repository, this.m_configFile);
            }
        }
    }
}

