namespace log4net.Config
{
    using log4net.Core;
    using log4net.Plugin;
    using log4net.Util;
    using System;

    [Serializable, AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public sealed class PluginAttribute : Attribute, IPluginFactory
    {
        private string m_typeName;
        private System.Type m_type;

        public PluginAttribute(string typeName)
        {
            this.m_typeName = typeName;
        }

        public PluginAttribute(System.Type type)
        {
            this.m_type = type;
        }

        public IPlugin CreatePlugin()
        {
            System.Type c = this.m_type;
            if (this.m_type == null)
            {
                c = SystemInfo.GetTypeFromString(this.m_typeName, true, true);
            }
            if (!typeof(IPlugin).IsAssignableFrom(c))
            {
                throw new LogException("Plugin type [" + c.FullName + "] does not implement the log4net.IPlugin interface");
            }
            return (IPlugin) Activator.CreateInstance(c);
        }

        public override string ToString() => 
            (this.m_type == null) ? ("PluginAttribute[Type=" + this.m_typeName + "]") : ("PluginAttribute[Type=" + this.m_type.FullName + "]");

        public System.Type Type
        {
            get => 
                this.m_type;
            set => 
                this.m_type = value;
        }

        public string TypeName
        {
            get => 
                this.m_typeName;
            set => 
                this.m_typeName = value;
        }
    }
}

