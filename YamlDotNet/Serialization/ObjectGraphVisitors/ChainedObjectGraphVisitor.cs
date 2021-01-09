namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
    using System;
    using YamlDotNet.Serialization;

    public abstract class ChainedObjectGraphVisitor : IObjectGraphVisitor
    {
        private readonly IObjectGraphVisitor nextVisitor;

        protected ChainedObjectGraphVisitor(IObjectGraphVisitor nextVisitor)
        {
            this.nextVisitor = nextVisitor;
        }

        public virtual bool Enter(IObjectDescriptor value) => 
            this.nextVisitor.Enter(value);

        public virtual bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value) => 
            this.nextVisitor.EnterMapping(key, value);

        public virtual bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value) => 
            this.nextVisitor.EnterMapping(key, value);

        public virtual void VisitMappingEnd(IObjectDescriptor mapping)
        {
            this.nextVisitor.VisitMappingEnd(mapping);
        }

        public virtual void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType)
        {
            this.nextVisitor.VisitMappingStart(mapping, keyType, valueType);
        }

        public virtual void VisitScalar(IObjectDescriptor scalar)
        {
            this.nextVisitor.VisitScalar(scalar);
        }

        public virtual void VisitSequenceEnd(IObjectDescriptor sequence)
        {
            this.nextVisitor.VisitSequenceEnd(sequence);
        }

        public virtual void VisitSequenceStart(IObjectDescriptor sequence, Type elementType)
        {
            this.nextVisitor.VisitSequenceStart(sequence, elementType);
        }
    }
}

