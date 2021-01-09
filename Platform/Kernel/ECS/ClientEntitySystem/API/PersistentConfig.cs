namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;
    using System.Runtime.InteropServices;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public class PersistentConfig : Attribute
    {
        internal readonly string value;
        internal readonly bool configOptional;

        public PersistentConfig(string value = "", bool configOptional = false)
        {
            this.value = value;
            this.configOptional = configOptional;
        }
    }
}

