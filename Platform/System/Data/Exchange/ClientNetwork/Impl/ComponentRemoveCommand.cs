namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;

    public class ComponentRemoveCommand : ComponentCommand
    {
        public ComponentRemoveCommand()
        {
        }

        public ComponentRemoveCommand(Entity entity, Type componentType) : base(entity, componentType)
        {
        }

        public override void Execute(Engine engine)
        {
            base.Entity.RemoveComponentSilent(base.ComponentType);
        }

        public ComponentRemoveCommand Init(Entity entity, Type componentType)
        {
            base.ComponentType = componentType;
            base.Entity = (EntityInternal) entity;
            return this;
        }

        public override string ToString() => 
            $"ComponentRemoveCommand Entity={base.Entity} ComponentType={base.ComponentType}";
    }
}

