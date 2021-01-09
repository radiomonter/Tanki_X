namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class UnsupportedParameterException : Exception
    {
        public UnsupportedParameterException(Type type) : base("type = " + type)
        {
        }
    }
}

