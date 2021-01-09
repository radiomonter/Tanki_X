namespace log4net.Util
{
    using System;
    using System.Reflection;

    public sealed class ThreadContextProperties : ContextPropertiesBase
    {
        [ThreadStatic]
        private static PropertiesDictionary _dictionary;

        internal ThreadContextProperties()
        {
        }

        public void Clear()
        {
            if (_dictionary != null)
            {
                _dictionary.Clear();
            }
        }

        public string[] GetKeys() => 
            _dictionary?.GetKeys();

        internal PropertiesDictionary GetProperties(bool create)
        {
            if ((_dictionary == null) && create)
            {
                _dictionary = new PropertiesDictionary();
            }
            return _dictionary;
        }

        public void Remove(string key)
        {
            if (_dictionary != null)
            {
                _dictionary.Remove(key);
            }
        }

        public override object this[string key]
        {
            get => 
                _dictionary?[key];
            set => 
                this.GetProperties(true)[key] = value;
        }
    }
}

