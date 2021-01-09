namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using YamlDotNet.Core;

    [Serializable]
    internal class YamlAliasNode : YamlNode
    {
        internal YamlAliasNode(string anchor)
        {
            base.Anchor = anchor;
        }

        public override void Accept(IYamlVisitor visitor)
        {
            throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be visited.");
        }

        internal override void Emit(IEmitter emitter, EmitterState state)
        {
            throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be saved.");
        }

        public override bool Equals(object other)
        {
            YamlAliasNode node = other as YamlAliasNode;
            return (((node != null) && base.Equals(node)) && SafeEquals(base.Anchor, node.Anchor));
        }

        public override int GetHashCode() => 
            base.GetHashCode();

        internal override void ResolveAliases(DocumentLoadingState state)
        {
            throw new NotSupportedException("Resolving an alias on an alias node does not make sense");
        }

        public override string ToString() => 
            "*" + base.Anchor;

        public override IEnumerable<YamlNode> AllNodes =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        public override YamlNodeType NodeType =>
            YamlNodeType.Alias;

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<YamlNode>, IEnumerator, IDisposable, IEnumerator<YamlNode>
        {
            internal YamlAliasNode $this;
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
                return new YamlAliasNode.<>c__Iterator0 { $this = this.$this };
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

