namespace YamlDotNet.Serialization
{
    using System;

    public interface IObjectGraphVisitor
    {
        bool Enter(IObjectDescriptor value);
        bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value);
        bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value);
        void VisitMappingEnd(IObjectDescriptor mapping);
        void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType);
        void VisitScalar(IObjectDescriptor scalar);
        void VisitSequenceEnd(IObjectDescriptor sequence);
        void VisitSequenceStart(IObjectDescriptor sequence, Type elementType);
    }
}

