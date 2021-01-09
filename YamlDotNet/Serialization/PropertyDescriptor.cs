namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core;

    public sealed class PropertyDescriptor : IPropertyDescriptor
    {
        private readonly IPropertyDescriptor baseDescriptor;

        public PropertyDescriptor(IPropertyDescriptor baseDescriptor)
        {
            this.baseDescriptor = baseDescriptor;
            this.Name = baseDescriptor.Name;
        }

        public T GetCustomAttribute<T>() where T: Attribute => 
            this.baseDescriptor.GetCustomAttribute<T>();

        public IObjectDescriptor Read(object target) => 
            this.baseDescriptor.Read(target);

        public void Write(object target, object value)
        {
            this.baseDescriptor.Write(target, value);
        }

        public string Name { get; set; }

        public System.Type Type =>
            this.baseDescriptor.Type;

        public System.Type TypeOverride
        {
            get => 
                this.baseDescriptor.TypeOverride;
            set => 
                this.baseDescriptor.TypeOverride = value;
        }

        public int Order { get; set; }

        public YamlDotNet.Core.ScalarStyle ScalarStyle
        {
            get => 
                this.baseDescriptor.ScalarStyle;
            set => 
                this.baseDescriptor.ScalarStyle = value;
        }

        public bool CanWrite =>
            this.baseDescriptor.CanWrite;
    }
}

