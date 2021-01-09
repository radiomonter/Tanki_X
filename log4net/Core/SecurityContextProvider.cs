namespace log4net.Core
{
    using log4net.Util;
    using System;

    public class SecurityContextProvider
    {
        private static SecurityContextProvider s_defaultProvider = new SecurityContextProvider();

        protected SecurityContextProvider()
        {
        }

        public virtual SecurityContext CreateSecurityContext(object consumer) => 
            NullSecurityContext.Instance;

        public static SecurityContextProvider DefaultProvider
        {
            get => 
                s_defaultProvider;
            set => 
                s_defaultProvider = value;
        }
    }
}

