namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ComponentChangeCommand : EntityCommand
    {
        public ComponentChangeCommand()
        {
        }

        public ComponentChangeCommand(Entity entity, Platform.Kernel.ECS.ClientEntitySystem.API.Component component) : base(entity)
        {
            this.Component = component;
        }

        private void ApplyChange(Engine engine)
        {
            ((EntityImpl) base.Entity).ChangeComponent(this.Component);
        }

        public override void Execute(Engine engine)
        {
            this.ApplyChange(engine);
        }

        public ComponentChangeCommand Init(Entity entity, Platform.Kernel.ECS.ClientEntitySystem.API.Component component)
        {
            this.Component = component;
            base.Entity = (EntityInternal) entity;
            return this;
        }

        public override string ToString() => 
            $"ComponentChangeCommand Entity={base.Entity} Component={EcsToStringUtil.ToStringWithProperties(this.Component, ", ")}";

        [ProtocolVaried, ProtocolParameterOrder(1)]
        public Platform.Kernel.ECS.ClientEntitySystem.API.Component Component { get; set; }
    }
}

