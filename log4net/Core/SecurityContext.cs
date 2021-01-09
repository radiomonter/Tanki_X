namespace log4net.Core
{
    using System;

    public abstract class SecurityContext
    {
        protected SecurityContext()
        {
        }

        public abstract IDisposable Impersonate(object state);
    }
}

