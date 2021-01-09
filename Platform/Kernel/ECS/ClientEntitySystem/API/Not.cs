namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
    public class Not : Attribute
    {
        internal readonly Type value;

        public Not(Type type)
        {
            this.value = type;
        }
    }
}

