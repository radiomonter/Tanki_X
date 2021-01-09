namespace log4net.Util
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class CompositeProperties
    {
        private PropertiesDictionary m_flattened;
        private ArrayList m_nestedProperties = new ArrayList();

        internal CompositeProperties()
        {
        }

        public void Add(ReadOnlyPropertiesDictionary properties)
        {
            this.m_flattened = null;
            this.m_nestedProperties.Add(properties);
        }

        public PropertiesDictionary Flatten()
        {
            if (this.m_flattened == null)
            {
                this.m_flattened = new PropertiesDictionary();
                int count = this.m_nestedProperties.Count;
                while (--count >= 0)
                {
                    ReadOnlyPropertiesDictionary dictionary = (ReadOnlyPropertiesDictionary) this.m_nestedProperties[count];
                    IEnumerator enumerator = ((IEnumerable) dictionary).GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                            this.m_flattened[(string) current.Key] = current.Value;
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }
            return this.m_flattened;
        }

        public object this[string key]
        {
            get
            {
                object obj2;
                if (this.m_flattened != null)
                {
                    return this.m_flattened[key];
                }
                IEnumerator enumerator = this.m_nestedProperties.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            ReadOnlyPropertiesDictionary current = (ReadOnlyPropertiesDictionary) enumerator.Current;
                            if (!current.Contains(key))
                            {
                                continue;
                            }
                            obj2 = current[key];
                        }
                        else
                        {
                            return null;
                        }
                        break;
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                return obj2;
            }
        }
    }
}

