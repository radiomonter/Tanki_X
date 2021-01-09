namespace Platform.Library.ClientProtocol.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CodecInfoWithAttributes
    {
        private Dictionary<Type, object> attributes;

        public CodecInfoWithAttributes()
        {
            this.attributes = new Dictionary<Type, object>();
        }

        public CodecInfoWithAttributes(CodecInfo info) : this()
        {
            this.Info = info;
        }

        public CodecInfoWithAttributes(Type type, bool optional, bool varied) : this(new CodecInfo(type, optional, varied))
        {
        }

        public void AddAttribute(object attribute)
        {
            this.attributes.Add(attribute.GetType(), attribute);
        }

        public T GetAttribute<T>() where T: Attribute => 
            (T) this.attributes[typeof(T)];

        public bool IsAttributePresent<T>() where T: Attribute => 
            this.attributes.ContainsKey(typeof(T));

        public CodecInfo Info { get; private set; }
    }
}

