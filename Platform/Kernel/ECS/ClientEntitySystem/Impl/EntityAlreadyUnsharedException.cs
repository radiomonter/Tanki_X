namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class EntityAlreadyUnsharedException : Exception
    {
        public EntityAlreadyUnsharedException(Entity entity) : base("entity=" + entity)
        {
        }
    }
}

