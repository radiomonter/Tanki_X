namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public abstract class ComponentCommand : EntityCommand
    {
        public ComponentCommand()
        {
        }

        public ComponentCommand(Entity entity, Type componentType) : base(entity)
        {
            this.ComponentType = componentType;
        }

        protected bool Equals(ComponentCommand other) => 
            Equals(this.ComponentType, other.ComponentType);

        public override bool Equals(object obj) => 
            !ReferenceEquals(null, obj) ? (!ReferenceEquals(this, obj) ? (ReferenceEquals(obj.GetType(), base.GetType()) ? this.Equals((ComponentCommand) obj) : false) : true) : false;

        public override int GetHashCode() => 
            (this.ComponentType == null) ? 0 : this.ComponentType.GetHashCode();

        [ProtocolVaried, ProtocolParameterOrder(1)]
        public Type ComponentType { get; set; }
    }
}

