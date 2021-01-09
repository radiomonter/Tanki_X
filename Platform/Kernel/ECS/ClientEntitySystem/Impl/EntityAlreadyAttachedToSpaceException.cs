namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class EntityAlreadyAttachedToSpaceException : Exception
    {
        public EntityAlreadyAttachedToSpaceException(Entity entity) : base("entity=" + entity)
        {
        }

        public EntityAlreadyAttachedToSpaceException(EntityInternal entity, GroupComponent group) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "entity=", entity, " exists in attached group=", group };
        }
    }
}

