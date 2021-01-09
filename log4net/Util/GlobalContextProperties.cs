namespace log4net.Util
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public sealed class GlobalContextProperties : ContextPropertiesBase
    {
        private volatile ReadOnlyPropertiesDictionary m_readOnlyProperties = new ReadOnlyPropertiesDictionary();
        private readonly object m_syncRoot = new object();

        internal GlobalContextProperties()
        {
        }

        public void Clear()
        {
            lock (this.m_syncRoot)
            {
                this.m_readOnlyProperties = new ReadOnlyPropertiesDictionary();
            }
        }

        internal ReadOnlyPropertiesDictionary GetReadOnlyProperties() => 
            this.m_readOnlyProperties;

        public void Remove(string key)
        {
            lock (this.m_syncRoot)
            {
                if (this.m_readOnlyProperties.Contains(key))
                {
                    PropertiesDictionary propertiesDictionary = new PropertiesDictionary(this.m_readOnlyProperties);
                    propertiesDictionary.Remove(key);
                    this.m_readOnlyProperties = new ReadOnlyPropertiesDictionary(propertiesDictionary);
                }
            }
        }

        public override object this[string key]
        {
            get => 
                this.m_readOnlyProperties[key];
            set
            {
                lock (this.m_syncRoot)
                {
                    PropertiesDictionary propertiesDictionary = new PropertiesDictionary(this.m_readOnlyProperties) {
                        [key] = value
                    };
                    this.m_readOnlyProperties = new ReadOnlyPropertiesDictionary(propertiesDictionary);
                }
            }
        }
    }
}

