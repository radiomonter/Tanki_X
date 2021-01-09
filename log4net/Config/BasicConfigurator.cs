namespace log4net.Config
{
    using log4net;
    using log4net.Appender;
    using log4net.Layout;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class BasicConfigurator
    {
        private static readonly Type declaringType = typeof(BasicConfigurator);

        private BasicConfigurator()
        {
        }

        public static ICollection Configure() => 
            Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));

        public static ICollection Configure(IAppender appender)
        {
            IAppender[] appenders = new IAppender[] { appender };
            return Configure(appenders);
        }

        public static ICollection Configure(params IAppender[] appenders)
        {
            ArrayList items = new ArrayList();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigure(repository, appenders);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection Configure(ILoggerRepository repository)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                PatternLayout layout = new PatternLayout {
                    ConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline"
                };
                layout.ActivateOptions();
                ConsoleAppender appender = new ConsoleAppender {
                    Layout = layout
                };
                appender.ActivateOptions();
                IAppender[] appenders = new IAppender[] { appender };
                InternalConfigure(repository, appenders);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        public static ICollection Configure(ILoggerRepository repository, IAppender appender)
        {
            IAppender[] appenders = new IAppender[] { appender };
            return Configure(repository, appenders);
        }

        public static ICollection Configure(ILoggerRepository repository, params IAppender[] appenders)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                InternalConfigure(repository, appenders);
            }
            repository.ConfigurationMessages = items;
            return items;
        }

        private static void InternalConfigure(ILoggerRepository repository, params IAppender[] appenders)
        {
            IBasicRepositoryConfigurator configurator = repository as IBasicRepositoryConfigurator;
            if (configurator != null)
            {
                configurator.Configure(appenders);
            }
            else
            {
                LogLog.Warn(declaringType, "BasicConfigurator: Repository [" + repository + "] does not support the BasicConfigurator");
            }
        }
    }
}

