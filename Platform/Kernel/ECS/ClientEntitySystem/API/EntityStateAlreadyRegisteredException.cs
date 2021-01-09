namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public class EntityStateAlreadyRegisteredException : Exception
    {
        public EntityStateAlreadyRegisteredException(Type stateType) : base("State " + stateType + " is not registered")
        {
        }
    }
}

