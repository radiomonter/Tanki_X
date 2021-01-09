namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;

    [Serializable, DebuggerDisplay("{Value}")]
    public class YamlScalarNode : YamlNode
    {
        public YamlScalarNode()
        {
        }

        public YamlScalarNode(string value)
        {
            this.Value = value;
        }

        internal YamlScalarNode(EventReader events, DocumentLoadingState state)
        {
            Scalar yamlEvent = events.Expect<Scalar>();
            base.Load(yamlEvent, state);
            this.Value = yamlEvent.Value;
            this.Style = yamlEvent.Style;
        }

        public override void Accept(IYamlVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal override void Emit(IEmitter emitter, EmitterState state)
        {
            emitter.Emit(new Scalar(base.Anchor, base.Tag, this.Value, this.Style, true, false));
        }

        public override bool Equals(object other)
        {
            YamlScalarNode node = other as YamlScalarNode;
            return (((node != null) && base.Equals(node)) && SafeEquals(this.Value, node.Value));
        }

        public override int GetHashCode() => 
            CombineHashCodes(base.GetHashCode(), GetHashCode(this.Value));

        public static explicit operator string(YamlScalarNode value) => 
            value.Value;

        public static implicit operator YamlScalarNode(string value) => 
            new YamlScalarNode(value);

        internal override void ResolveAliases(DocumentLoadingState state)
        {
            throw new NotSupportedException("Resolving an alias on a scalar node does not make sense");
        }

        public override string ToString() => 
            this.Value;

        public string Value { get; set; }

        public ScalarStyle Style { get; set; }

        public override IEnumerable<YamlNode> AllNodes =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        public override YamlNodeType NodeType =>
            YamlNodeType.Scalar;

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<YamlNode>, IEnumerator, IDisposable, IEnumerator<YamlNode>
        {
            internal YamlScalarNode $this;
            internal YamlNode $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = this.$this;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$PC = -1;
                        break;

                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<YamlNode> IEnumerable<YamlNode>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new YamlScalarNode.<>c__Iterator0 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<YamlDotNet.RepresentationModel.YamlNode>.GetEnumerator();

            YamlNode IEnumerator<YamlNode>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

