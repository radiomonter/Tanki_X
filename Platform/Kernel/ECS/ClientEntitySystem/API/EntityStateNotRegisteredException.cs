namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public class EntityStateNotRegisteredException : Exception
    {
        public EntityStateNotRegisteredException(Type type) : base("State " + type + " is not registered")
        {
        }
    }
}

