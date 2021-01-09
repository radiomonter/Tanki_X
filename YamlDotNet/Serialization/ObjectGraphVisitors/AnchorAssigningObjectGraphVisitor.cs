namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
    using System;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;

    public sealed class AnchorAssigningObjectGraphVisitor : ChainedObjectGraphVisitor
    {
        private readonly IEventEmitter eventEmitter;
        private readonly IAliasProvider aliasProvider;
        private readonly HashSet<string> emittedAliases;

        public AnchorAssigningObjectGraphVisitor(IObjectGraphVisitor nextVisitor, IEventEmitter eventEmitter, IAliasProvider aliasProvider) : base(nextVisitor)
        {
            this.emittedAliases = new HashSet<string>();
            this.eventEmitter = eventEmitter;
            this.aliasProvider = aliasProvider;
        }

        public override bool Enter(IObjectDescriptor value)
        {
            string alias = this.aliasProvider.GetAlias(value.Value);
            if ((alias == null) || this.emittedAliases.Add(alias))
            {
                return base.Enter(value);
            }
            AliasEventInfo eventInfo = new AliasEventInfo(value) {
                Alias = alias
            };
            this.eventEmitter.Emit(eventInfo);
            return false;
        }

        public override void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType)
        {
            MappingStartEventInfo eventInfo = new MappingStartEventInfo(mapping) {
                Anchor = this.aliasProvider.GetAlias(mapping.Value)
            };
            this.eventEmitter.Emit(eventInfo);
        }

        public override void VisitScalar(IObjectDescriptor scalar)
        {
            ScalarEventInfo eventInfo = new ScalarEventInfo(scalar) {
                Anchor = this.aliasProvider.GetAlias(scalar.Value)
            };
            this.eventEmitter.Emit(eventInfo);
        }

        public override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType)
        {
            SequenceStartEventInfo eventInfo = new SequenceStartEventInfo(sequence) {
                Anchor = this.aliasProvider.GetAlias(sequence.Value)
            };
            this.eventEmitter.Emit(eventInfo);
        }
    }
}

