namespace log4net.Util
{
    using log4net.Core;
    using System;

    public sealed class NullSecurityContext : SecurityContext
    {
        public static readonly NullSecurityContext Instance = new NullSecurityContext();

        private NullSecurityContext()
        {
        }

        public override IDisposable Impersonate(object state) => 
            null;
    }
}

