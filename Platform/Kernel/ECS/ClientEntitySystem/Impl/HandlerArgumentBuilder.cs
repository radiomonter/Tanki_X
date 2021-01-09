namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;

    public class HandlerArgumentBuilder
    {
        private int position;
        private bool collection;
        private NodeClassInstanceDescription nodeClassInstanceDescription;
        private Optional<JoinType> joinType;
        private bool context;
        private bool mandatory;
        private bool combine;
        private bool optional;
        private Type type;

        public HandlerArgument Build() => 
            new HandlerArgument(this.position, this.collection, this.nodeClassInstanceDescription, this.joinType, this.context, this.mandatory, this.combine, this.optional, this.type);

        public HandlerArgumentBuilder SetCollection(bool collection)
        {
            this.collection = collection;
            return this;
        }

        public HandlerArgumentBuilder SetCombine(bool combine)
        {
            this.combine = combine;
            return this;
        }

        public HandlerArgumentBuilder SetContext(bool context)
        {
            this.context = context;
            return this;
        }

        public HandlerArgumentBuilder SetJoinType(Optional<JoinType> joinType)
        {
            this.joinType = joinType;
            return this;
        }

        public HandlerArgumentBuilder SetMandatory(bool mandatory)
        {
            this.mandatory = mandatory;
            return this;
        }

        public HandlerArgumentBuilder SetNodeClassInstanceDescription(NodeClassInstanceDescription nodeClassInstanceDescription)
        {
            this.nodeClassInstanceDescription = nodeClassInstanceDescription;
            return this;
        }

        public HandlerArgumentBuilder SetOptional(bool optional)
        {
            this.optional = optional;
            return this;
        }

        public HandlerArgumentBuilder SetPosition(int position)
        {
            this.position = position;
            return this;
        }

        public HandlerArgumentBuilder SetType(Type type)
        {
            this.type = type;
            return this;
        }
    }
}

