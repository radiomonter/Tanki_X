namespace log4net.Core
{
    using log4net.Repository;
    using System;
    using System.Reflection;

    public interface IRepositorySelector
    {
        event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent;

        ILoggerRepository CreateRepository(Assembly assembly, Type repositoryType);
        ILoggerRepository CreateRepository(string repositoryName, Type repositoryType);
        bool ExistsRepository(string repositoryName);
        ILoggerRepository[] GetAllRepositories();
        ILoggerRepository GetRepository(Assembly assembly);
        ILoggerRepository GetRepository(string repositoryName);
    }
}

