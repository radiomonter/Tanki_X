namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;
    using System.Runtime.InteropServices;

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple=false, Inherited=false)]
    public class OverrideConfigPathAttribute : Attribute
    {
        internal readonly string configPath;
        internal readonly PathOverrideType pathOverrideType;

        public OverrideConfigPathAttribute(string configPath, PathOverrideType overrideType = 0)
        {
            this.configPath = configPath;
            this.pathOverrideType = overrideType;
        }
    }
}

