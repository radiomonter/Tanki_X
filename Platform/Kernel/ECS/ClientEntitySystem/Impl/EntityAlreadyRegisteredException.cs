namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class EntityAlreadyRegisteredException : Exception
    {
        public EntityAlreadyRegisteredException(Entity newEntity) : base("entity=" + newEntity)
        {
        }
    }
}

