namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class EntityAlreadySharedException : Exception
    {
        public EntityAlreadySharedException(Entity entity) : base("entity=" + entity)
        {
        }
    }
}

