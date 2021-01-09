namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core;

    public sealed class ObjectDescriptor : IObjectDescriptor
    {
        public ObjectDescriptor(object value, System.Type type, System.Type staticType) : this(value, type, staticType, YamlDotNet.Core.ScalarStyle.Any)
        {
        }

        public ObjectDescriptor(object value, System.Type type, System.Type staticType, YamlDotNet.Core.ScalarStyle scalarStyle)
        {
            this.Value = value;
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            this.Type = type;
            if (staticType == null)
            {
                throw new ArgumentNullException("staticType");
            }
            this.StaticType = staticType;
            this.ScalarStyle = scalarStyle;
        }

        public object Value { get; private set; }

        public System.Type Type { get; private set; }

        public System.Type StaticType { get; private set; }

        public YamlDotNet.Core.ScalarStyle ScalarStyle { get; private set; }
    }
}

