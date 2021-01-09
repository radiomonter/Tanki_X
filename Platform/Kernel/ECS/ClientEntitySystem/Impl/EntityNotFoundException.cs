namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(long entityId) : base("entityId = " + entityId)
        {
        }
    }
}

