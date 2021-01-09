namespace Platform.Library.ClientLogger.Impl
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.Filter;
    using log4net.Layout;
    using System;
    using System.Collections.Generic;

    public abstract class AppenderBuilder
    {
        private const string DEFAULT_LAYOUT = "%d{ABSOLUTE} %5p %c - %m%n";
        private AppenderSkeleton appender;
        private List<IFilter> excludeFilters = new List<IFilter>();
        private List<IFilter> includeFilters = new List<IFilter>();

        protected AppenderBuilder()
        {
        }

        public AppenderBuilder AddExcludeFilter(IFilter filter)
        {
            this.excludeFilters.Add(filter);
            return this;
        }

        public AppenderBuilder AddIncludeFilter(IFilter filter)
        {
            this.includeFilters.Add(filter);
            return this;
        }

        public AppenderBuilder AddLoggerMatchExculdeFilter(string matchString)
        {
            this.AddLoggerMatchFilter(matchString, false);
            return this;
        }

        public AppenderBuilder AddLoggerMatchExculdeFilter(Type matchType)
        {
            this.AddLoggerMatchExculdeFilter(matchType.FullName);
            return this;
        }

        public AppenderBuilder AddLoggerMatchFilter(string matchString)
        {
            this.AddLoggerMatchFilter(matchString, true);
            return this;
        }

        public AppenderBuilder AddLoggerMatchFilter(Type matchType) => 
            this.AddLoggerMatchFilter(matchType.FullName);

        private void AddLoggerMatchFilter(string matchString, bool include)
        {
            LoggerMatchFilter filter = new LoggerMatchFilter {
                LoggerToMatch = matchString,
                AcceptOnMatch = include
            };
            if (include)
            {
                this.AddIncludeFilter(filter);
            }
            else
            {
                this.AddExcludeFilter(filter);
            }
        }

        public AppenderBuilder ClearFilters()
        {
            this.appender.ClearFilters();
            return this;
        }

        public AppenderSkeleton Configure()
        {
            if (this.appender.Layout == null)
            {
                this.SetLayout("%d{ABSOLUTE} %5p %c - %m%n");
            }
            foreach (IFilter filter in this.excludeFilters)
            {
                this.appender.AddFilter(filter);
            }
            foreach (IFilter filter2 in this.includeFilters)
            {
                this.appender.AddFilter(filter2);
            }
            if (this.includeFilters.Count > 0)
            {
                this.appender.AddFilter(new log4net.Filter.DenyAllFilter());
            }
            return this.appender;
        }

        public AppenderBuilder DenyAllFilter()
        {
            this.includeFilters.Add(new log4net.Filter.DenyAllFilter());
            return this;
        }

        protected void Init(AppenderSkeleton appender)
        {
            this.appender = appender;
        }

        public AppenderBuilder SetLayout(ILayout layout)
        {
            this.appender.Layout = layout;
            return this;
        }

        public AppenderBuilder SetLayout(string layoutString)
        {
            PatternLayout layout = new PatternLayout(layoutString);
            return this.SetLayout(layout);
        }

        public AppenderBuilder SetLevel(Level level)
        {
            this.appender.Threshold = level;
            return this;
        }

        public AppenderBuilder SetName(string appenderName)
        {
            this.appender.Name = appenderName;
            return this;
        }
    }
}

