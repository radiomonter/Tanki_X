namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HandlerArgument
    {
        public HandlerArgument(int nodeNumber, bool collection, NodeClassInstanceDescription nodeClassInstanceDescription, Optional<Platform.Kernel.ECS.ClientEntitySystem.Impl.JoinType> joinJoinType, bool context, bool mandatory, bool combine, bool optional, Type argumentType)
        {
            this.NodeNumber = nodeNumber;
            this.Collection = collection;
            this.ClassInstanceDescription = nodeClassInstanceDescription;
            this.JoinType = joinJoinType;
            this.Context = context;
            this.Mandatory = mandatory;
            this.Combine = combine;
            this.Optional = optional;
            this.ArgumentType = argumentType;
            this.SelectAll = !this.JoinType.IsPresent() || (this.JoinType.IsPresent() && ReferenceEquals(this.JoinType.Get().GetType(), typeof(JoinAllType)));
            this.Validate();
        }

        public override string ToString() => 
            $"{base.GetType()}[NodeNumber={this.NodeNumber}, Collection={this.Collection}, ArgumentType={this.ArgumentType}]";

        private void Validate()
        {
            if (this.Optional && this.Collection)
            {
                throw new OptionalCollectionNotValidException();
            }
        }

        public int NodeNumber { get; internal set; }

        public bool Collection { get; internal set; }

        public NodeClassInstanceDescription ClassInstanceDescription { get; internal set; }

        public Optional<Platform.Kernel.ECS.ClientEntitySystem.Impl.JoinType> JoinType { get; internal set; }

        public bool Context { get; internal set; }

        public bool Mandatory { get; internal set; }

        public bool Combine { get; internal set; }

        public bool Optional { get; internal set; }

        public Type ArgumentType { get; set; }

        public bool SelectAll { get; internal set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeDescription NodeDescription =>
            this.ClassInstanceDescription.NodeDescription;
    }
}

