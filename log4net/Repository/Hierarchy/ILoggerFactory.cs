namespace log4net.Repository.Hierarchy
{
    using log4net.Repository;
    using System;

    public interface ILoggerFactory
    {
        Logger CreateLogger(ILoggerRepository repository, string name);
    }
}

