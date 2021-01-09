namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using YamlDotNet;
    using YamlDotNet.Serialization;

    public sealed class AnchorAssigner : IObjectGraphVisitor, IAliasProvider
    {
        private readonly IDictionary<object, AnchorAssignment> assignments = new Dictionary<object, AnchorAssignment>();
        private uint nextId;

        string IAliasProvider.GetAlias(object target)
        {
            AnchorAssignment assignment;
            return (((target == null) || !this.assignments.TryGetValue(target, out assignment)) ? null : assignment.Anchor);
        }

        bool IObjectGraphVisitor.Enter(IObjectDescriptor value)
        {
            if ((value.Value != null) && (value.Type.GetTypeCode() == TypeCode.Object))
            {
                AnchorAssignment assignment;
                if (!this.assignments.TryGetValue(value.Value, out assignment))
                {
                    this.assignments.Add(value.Value, new AnchorAssignment());
                    return true;
                }
                if (assignment.Anchor == null)
                {
                    assignment.Anchor = "o" + this.nextId.ToString(CultureInfo.InvariantCulture);
                    this.nextId++;
                }
            }
            return false;
        }

        bool IObjectGraphVisitor.EnterMapping(IObjectDescriptor key, IObjectDescriptor value) => 
            true;

        bool IObjectGraphVisitor.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value) => 
            true;

        void IObjectGraphVisitor.VisitMappingEnd(IObjectDescriptor mapping)
        {
        }

        void IObjectGraphVisitor.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType)
        {
        }

        void IObjectGraphVisitor.VisitScalar(IObjectDescriptor scalar)
        {
        }

        void IObjectGraphVisitor.VisitSequenceEnd(IObjectDescriptor sequence)
        {
        }

        void IObjectGraphVisitor.VisitSequenceStart(IObjectDescriptor sequence, Type elementType)
        {
        }

        private class AnchorAssignment
        {
            public string Anchor;
        }
    }
}

