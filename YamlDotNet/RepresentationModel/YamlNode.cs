namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;

    [Serializable]
    public abstract class YamlNode
    {
        protected YamlNode()
        {
        }

        public abstract void Accept(IYamlVisitor visitor);
        protected static int CombineHashCodes(int h1, int h2) => 
            ((h1 << 5) + h1) ^ h2;

        internal abstract void Emit(IEmitter emitter, EmitterState state);
        protected bool Equals(YamlNode other) => 
            SafeEquals(this.Tag, other.Tag);

        public override int GetHashCode() => 
            GetHashCode(this.Tag);

        protected static int GetHashCode(object value) => 
            (value != null) ? value.GetHashCode() : 0;

        internal void Load(NodeEvent yamlEvent, DocumentLoadingState state)
        {
            this.Tag = yamlEvent.Tag;
            if (yamlEvent.Anchor != null)
            {
                this.Anchor = yamlEvent.Anchor;
                state.AddAnchor(this);
            }
            this.Start = yamlEvent.Start;
            this.End = yamlEvent.End;
        }

        internal static YamlNode ParseNode(EventReader events, DocumentLoadingState state)
        {
            if (events.Accept<Scalar>())
            {
                return new YamlScalarNode(events, state);
            }
            if (events.Accept<SequenceStart>())
            {
                return new YamlSequenceNode(events, state);
            }
            if (events.Accept<MappingStart>())
            {
                return new YamlMappingNode(events, state);
            }
            if (!events.Accept<AnchorAlias>())
            {
                throw new ArgumentException("The current event is of an unsupported type.", "events");
            }
            AnchorAlias alias = events.Expect<AnchorAlias>();
            return (state.GetNode(alias.Value, false, alias.Start, alias.End) ?? new YamlAliasNode(alias.Value));
        }

        internal abstract void ResolveAliases(DocumentLoadingState state);
        protected static bool SafeEquals(object first, object second) => 
            (first == null) ? ((second == null) || second.Equals(first)) : first.Equals(second);

        internal void Save(IEmitter emitter, EmitterState state)
        {
            if (!string.IsNullOrEmpty(this.Anchor) && !state.EmittedAnchors.Add(this.Anchor))
            {
                emitter.Emit(new AnchorAlias(this.Anchor));
            }
            else
            {
                this.Emit(emitter, state);
            }
        }

        public string Anchor { get; set; }

        public string Tag { get; set; }

        public Mark Start { get; private set; }

        public Mark End { get; private set; }

        public abstract IEnumerable<YamlNode> AllNodes { get; }

        public abstract YamlNodeType NodeType { get; }
    }
}

