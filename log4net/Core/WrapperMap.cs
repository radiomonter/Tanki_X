namespace log4net.Core
{
    using log4net.Repository;
    using System;
    using System.Collections;

    public class WrapperMap
    {
        private readonly Hashtable m_repositories = new Hashtable();
        private readonly WrapperCreationHandler m_createWrapperHandler;
        private readonly LoggerRepositoryShutdownEventHandler m_shutdownHandler;

        public WrapperMap(WrapperCreationHandler createWrapperHandler)
        {
            this.m_createWrapperHandler = createWrapperHandler;
            this.m_shutdownHandler = new LoggerRepositoryShutdownEventHandler(this.ILoggerRepository_Shutdown);
        }

        protected virtual ILoggerWrapper CreateNewWrapperObject(ILogger logger) => 
            this.m_createWrapperHandler?.Invoke(logger);

        public virtual ILoggerWrapper GetWrapper(ILogger logger)
        {
            if (logger == null)
            {
                return null;
            }
            lock (this)
            {
                Hashtable hashtable = (Hashtable) this.m_repositories[logger.Repository];
                if (hashtable == null)
                {
                    hashtable = new Hashtable();
                    this.m_repositories[logger.Repository] = hashtable;
                    logger.Repository.ShutdownEvent += this.m_shutdownHandler;
                }
                ILoggerWrapper wrapper = hashtable[logger] as ILoggerWrapper;
                if (wrapper == null)
                {
                    wrapper = this.CreateNewWrapperObject(logger);
                    hashtable[logger] = wrapper;
                }
                return wrapper;
            }
        }

        private void ILoggerRepository_Shutdown(object sender, EventArgs e)
        {
            ILoggerRepository repository = sender as ILoggerRepository;
            if (repository != null)
            {
                this.RepositoryShutdown(repository);
            }
        }

        protected virtual void RepositoryShutdown(ILoggerRepository repository)
        {
            lock (this)
            {
                this.m_repositories.Remove(repository);
                repository.ShutdownEvent -= this.m_shutdownHandler;
            }
        }

        protected Hashtable Repositories =>
            this.m_repositories;
    }
}

