namespace log4net.Util.TypeConverters
{
    using log4net.Util;
    using System;

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Class)]
    public sealed class TypeConverterAttribute : Attribute
    {
        private string m_typeName;

        public TypeConverterAttribute()
        {
        }

        public TypeConverterAttribute(string typeName)
        {
            this.m_typeName = typeName;
        }

        public TypeConverterAttribute(Type converterType)
        {
            this.m_typeName = SystemInfo.AssemblyQualifiedName(converterType);
        }

        public string ConverterTypeName
        {
            get => 
                this.m_typeName;
            set => 
                this.m_typeName = value;
        }
    }
}

