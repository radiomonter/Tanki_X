namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ComponentAddCommand : EntityCommand
    {
        public ComponentAddCommand()
        {
        }

        public ComponentAddCommand(Entity entity, Platform.Kernel.ECS.ClientEntitySystem.API.Component component) : base(entity)
        {
            this.Component = component;
        }

        protected bool Equals(ComponentAddCommand other) => 
            ReferenceEquals(this.Component, other.Component);

        public override bool Equals(object obj) => 
            !ReferenceEquals(null, obj) ? (!ReferenceEquals(this, obj) ? (ReferenceEquals(obj.GetType(), base.GetType()) ? this.Equals((ComponentAddCommand) obj) : false) : true) : false;

        public override void Execute(Engine engine)
        {
            base.Entity.AddComponentSilent(this.Component);
        }

        public override int GetHashCode() => 
            0;

        public ComponentAddCommand Init(Entity entity, Platform.Kernel.ECS.ClientEntitySystem.API.Component component)
        {
            this.Component = component;
            base.Entity = (EntityInternal) entity;
            return this;
        }

        public override string ToString() => 
            $"ComponentAddCommand: Entity={base.Entity} Component={this.Component}";

        [ProtocolVaried, ProtocolParameterOrder(1)]
        public Platform.Kernel.ECS.ClientEntitySystem.API.Component Component { get; set; }
    }
}

