namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class JoinBy : Attribute
    {
        internal readonly Type value;

        public JoinBy(Type value)
        {
            this.value = value;
        }
    }
}

