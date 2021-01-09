namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class ComponentAlreadyExistsInEntityException : Exception
    {
        public ComponentAlreadyExistsInEntityException(EntityInternal entity, Type componentClass) : base($"{componentClass.Name} entity={entity}")
        {
        }
    }
}

