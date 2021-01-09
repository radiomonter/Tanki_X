namespace log4net.Util
{
    using System;

    public sealed class ConverterInfo
    {
        private string m_name;
        private System.Type m_type;
        private readonly PropertiesDictionary properties = new PropertiesDictionary();

        public void AddProperty(PropertyEntry entry)
        {
            this.properties[entry.Key] = entry.Value;
        }

        public string Name
        {
            get => 
                this.m_name;
            set => 
                this.m_name = value;
        }

        public System.Type Type
        {
            get => 
                this.m_type;
            set => 
                this.m_type = value;
        }

        public PropertiesDictionary Properties =>
            this.properties;
    }
}

