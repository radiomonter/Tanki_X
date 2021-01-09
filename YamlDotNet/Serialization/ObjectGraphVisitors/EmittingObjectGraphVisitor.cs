namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
    using System;
    using YamlDotNet.Serialization;

    public sealed class EmittingObjectGraphVisitor : IObjectGraphVisitor
    {
        private readonly IEventEmitter eventEmitter;

        public EmittingObjectGraphVisitor(IEventEmitter eventEmitter)
        {
            this.eventEmitter = eventEmitter;
        }

        bool IObjectGraphVisitor.Enter(IObjectDescriptor value) => 
            true;

        bool IObjectGraphVisitor.EnterMapping(IObjectDescriptor key, IObjectDescriptor value) => 
            true;

        bool IObjectGraphVisitor.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value) => 
            true;

        void IObjectGraphVisitor.VisitMappingEnd(IObjectDescriptor mapping)
        {
            this.eventEmitter.Emit(new MappingEndEventInfo(mapping));
        }

        void IObjectGraphVisitor.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType)
        {
            this.eventEmitter.Emit(new MappingStartEventInfo(mapping));
        }

        void IObjectGraphVisitor.VisitScalar(IObjectDescriptor scalar)
        {
            this.eventEmitter.Emit(new ScalarEventInfo(scalar));
        }

        void IObjectGraphVisitor.VisitSequenceEnd(IObjectDescriptor sequence)
        {
            this.eventEmitter.Emit(new SequenceEndEventInfo(sequence));
        }

        void IObjectGraphVisitor.VisitSequenceStart(IObjectDescriptor sequence, Type elementType)
        {
            this.eventEmitter.Emit(new SequenceStartEventInfo(sequence));
        }
    }
}

