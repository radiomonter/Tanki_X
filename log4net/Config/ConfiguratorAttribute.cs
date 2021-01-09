namespace log4net.Config
{
    using log4net.Repository;
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Assembly)]
    public abstract class ConfiguratorAttribute : Attribute, IComparable
    {
        private int m_priority;

        protected ConfiguratorAttribute(int priority)
        {
            this.m_priority = priority;
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return 0;
            }
            int num = -1;
            ConfiguratorAttribute attribute = obj as ConfiguratorAttribute;
            if (attribute != null)
            {
                num = attribute.m_priority.CompareTo(this.m_priority);
                num ??= -1;
            }
            return num;
        }

        public abstract void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository);
    }
}

