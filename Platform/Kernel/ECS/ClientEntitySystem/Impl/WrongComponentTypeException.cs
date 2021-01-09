namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class WrongComponentTypeException : Exception
    {
        public WrongComponentTypeException(Type componentType) : base("componentType=" + componentType)
        {
        }
    }
}

