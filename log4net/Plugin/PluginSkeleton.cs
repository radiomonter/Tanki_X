namespace log4net.Plugin
{
    using log4net.Repository;
    using System;

    public abstract class PluginSkeleton : IPlugin
    {
        private string m_name;
        private ILoggerRepository m_repository;

        protected PluginSkeleton(string name)
        {
            this.m_name = name;
        }

        public virtual void Attach(ILoggerRepository repository)
        {
            this.m_repository = repository;
        }

        public virtual void Shutdown()
        {
        }

        public virtual string Name
        {
            get => 
                this.m_name;
            set => 
                this.m_name = value;
        }

        protected virtual ILoggerRepository LoggerRepository
        {
            get => 
                this.m_repository;
            set => 
                this.m_repository = value;
        }
    }
}

