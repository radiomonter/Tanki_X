namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException(Entity entity, Type componentClass) : base($"{componentClass.Name} entity={entity}")
        {
        }
    }
}

