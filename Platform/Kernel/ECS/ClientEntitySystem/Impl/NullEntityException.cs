namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NullEntityException : Exception
    {
        public NullEntityException() : base("Events can not be scheduled on null entities")
        {
        }
    }
}

