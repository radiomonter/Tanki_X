namespace log4net.Util
{
    using System;
    using System.Reflection;

    public abstract class ContextPropertiesBase
    {
        protected ContextPropertiesBase()
        {
        }

        public abstract object this[string key] { get; set; }
    }
}

