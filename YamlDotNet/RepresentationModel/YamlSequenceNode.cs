namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;

    [Serializable, DebuggerDisplay("Count = {children.Count}")]
    public class YamlSequenceNode : YamlNode, IEnumerable<YamlNode>, IEnumerable
    {
        private readonly IList<YamlNode> children;

        public YamlSequenceNode()
        {
            this.children = new List<YamlNode>();
        }

        public YamlSequenceNode(IEnumerable<YamlNode> children)
        {
            this.children = new List<YamlNode>();
            foreach (YamlNode node in children)
            {
                this.children.Add(node);
            }
        }

        public YamlSequenceNode(params YamlNode[] children) : this((IEnumerable<YamlNode>) children)
        {
        }

        internal YamlSequenceNode(EventReader events, DocumentLoadingState state)
        {
            this.children = new List<YamlNode>();
            SequenceStart yamlEvent = events.Expect<SequenceStart>();
            base.Load(yamlEvent, state);
            bool flag = false;
            while (!events.Accept<SequenceEnd>())
            {
                YamlNode item = ParseNode(events, state);
                this.children.Add(item);
                flag |= item is YamlAliasNode;
            }
            if (flag)
            {
                state.AddNodeWithUnresolvedAliases(this);
            }
            events.Expect<SequenceEnd>();
        }

        public override void Accept(IYamlVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Add(string child)
        {
            this.children.Add(new YamlScalarNode(child));
        }

        public void Add(YamlNode child)
        {
            this.children.Add(child);
        }

        internal override void Emit(IEmitter emitter, EmitterState state)
        {
            emitter.Emit(new SequenceStart(base.Anchor, base.Tag, true, this.Style));
            foreach (YamlNode node in this.children)
            {
                node.Save(emitter, state);
            }
            emitter.Emit(new SequenceEnd());
        }

        public override bool Equals(object other)
        {
            YamlSequenceNode node = other as YamlSequenceNode;
            if ((node == null) || (!base.Equals(node) || (this.children.Count != node.children.Count)))
            {
                return false;
            }
            for (int i = 0; i < this.children.Count; i++)
            {
                if (!SafeEquals(this.children[i], node.children[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerator<YamlNode> GetEnumerator() => 
            this.Children.GetEnumerator();

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            foreach (YamlNode node in this.children)
            {
                hashCode = CombineHashCodes(hashCode, GetHashCode(node));
            }
            return hashCode;
        }

        internal override void ResolveAliases(DocumentLoadingState state)
        {
            for (int i = 0; i < this.children.Count; i++)
            {
                if (this.children[i] is YamlAliasNode)
                {
                    this.children[i] = state.GetNode(this.children[i].Anchor, true, this.children[i].Start, this.children[i].End);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("[ ");
            foreach (YamlNode node in this.children)
            {
                if (builder.Length > 2)
                {
                    builder.Append(", ");
                }
                builder.Append(node);
            }
            builder.Append(" ]");
            return builder.ToString();
        }

        public IList<YamlNode> Children =>
            this.children;

        public SequenceStyle Style { get; set; }

        public override IEnumerable<YamlNode> AllNodes =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        public override YamlNodeType NodeType =>
            YamlNodeType.Sequence;

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<YamlNode>, IEnumerator, IDisposable, IEnumerator<YamlNode>
        {
            internal IEnumerator<YamlNode> $locvar0;
            internal YamlNode <child>__1;
            internal IEnumerator<YamlNode> $locvar1;
            internal YamlNode <node>__2;
            internal YamlSequenceNode $this;
            internal YamlNode $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 2:
                        try
                        {
                            try
                            {
                            }
                            finally
                            {
                                if (this.$locvar1 != null)
                                {
                                    this.$locvar1.Dispose();
                                }
                            }
                        }
                        finally
                        {
                            if (this.$locvar0 != null)
                            {
                                this.$locvar0.Dispose();
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$current = this.$this;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto TR_0001;

                    case 1:
                        this.$locvar0 = this.$this.children.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 2:
                        break;

                    default:
                        goto TR_0000;
                }
                try
                {
                    switch (num)
                    {
                        case 2:
                            goto TR_0016;

                        default:
                            break;
                    }
                    goto TR_0019;
                TR_0016:
                    try
                    {
                        switch (num)
                        {
                            default:
                                if (!this.$locvar1.MoveNext())
                                {
                                    break;
                                }
                                this.<node>__2 = this.$locvar1.Current;
                                this.$current = this.<node>__2;
                                if (!this.$disposing)
                                {
                                    this.$PC = 2;
                                }
                                flag = true;
                                goto TR_0001;
                        }
                    }
                    finally
                    {
                        if (flag)
                        {
                        }
                        if (this.$locvar1 != null)
                        {
                            this.$locvar1.Dispose();
                        }
                    }
                TR_0019:
                    while (true)
                    {
                        if (this.$locvar0.MoveNext())
                        {
                            this.<child>__1 = this.$locvar0.Current;
                            this.$locvar1 = this.<child>__1.AllNodes.GetEnumerator();
                            num = 0xfffffffd;
                        }
                        else
                        {
                            this.$PC = -1;
                            goto TR_0000;
                        }
                        break;
                    }
                    goto TR_0016;
                }
                finally
                {
                    if (flag)
                    {
                    }
                    if (this.$locvar0 != null)
                    {
                        this.$locvar0.Dispose();
                    }
                }
            TR_0000:
                return false;
            TR_0001:
                return true;
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
                return new YamlSequenceNode.<>c__Iterator0 { $this = this.$this };
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

