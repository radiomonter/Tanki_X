namespace log4net.Config
{
    using log4net.Core;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Reflection;

    [Serializable, AttributeUsage(AttributeTargets.Assembly)]
    public sealed class SecurityContextProviderAttribute : ConfiguratorAttribute
    {
        private Type m_providerType;
        private static readonly Type declaringType = typeof(SecurityContextProviderAttribute);

        public SecurityContextProviderAttribute(Type providerType) : base(100)
        {
            this.m_providerType = providerType;
        }

        public override void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository)
        {
            if (this.m_providerType == null)
            {
                LogLog.Error(declaringType, "Attribute specified on assembly [" + sourceAssembly.FullName + "] with null ProviderType.");
            }
            else
            {
                LogLog.Debug(declaringType, "Creating provider of type [" + this.m_providerType.FullName + "]");
                SecurityContextProvider provider = Activator.CreateInstance(this.m_providerType) as SecurityContextProvider;
                if (provider == null)
                {
                    LogLog.Error(declaringType, "Failed to create SecurityContextProvider instance of type [" + this.m_providerType.Name + "].");
                }
                else
                {
                    SecurityContextProvider.DefaultProvider = provider;
                }
            }
        }

        public Type ProviderType
        {
            get => 
                this.m_providerType;
            set => 
                this.m_providerType = value;
        }
    }
}

