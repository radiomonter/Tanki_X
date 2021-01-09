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

    [Serializable]
    public class YamlMappingNode : YamlNode, IEnumerable<KeyValuePair<YamlNode, YamlNode>>, IEnumerable
    {
        private readonly IDictionary<YamlNode, YamlNode> children;

        public YamlMappingNode()
        {
            this.children = new Dictionary<YamlNode, YamlNode>();
        }

        public YamlMappingNode(IEnumerable<KeyValuePair<YamlNode, YamlNode>> children)
        {
            this.children = new Dictionary<YamlNode, YamlNode>();
            foreach (KeyValuePair<YamlNode, YamlNode> pair in children)
            {
                this.children.Add(pair);
            }
        }

        public YamlMappingNode(IEnumerable<YamlNode> children)
        {
            this.children = new Dictionary<YamlNode, YamlNode>();
            foreach (YamlNode node in children)
            {
                IEnumerator<YamlNode> enumerator;
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentException("When constructing a mapping node with a sequence, the number of elements of the sequence must be even.");
                }
                this.Add(node, enumerator.Current);
            }
        }

        public YamlMappingNode(params KeyValuePair<YamlNode, YamlNode>[] children) : this((IEnumerable<KeyValuePair<YamlNode, YamlNode>>) children)
        {
        }

        public YamlMappingNode(params YamlNode[] children) : this((IEnumerable<YamlNode>) children)
        {
        }

        internal YamlMappingNode(EventReader events, DocumentLoadingState state)
        {
            this.children = new Dictionary<YamlNode, YamlNode>();
            MappingStart yamlEvent = events.Expect<MappingStart>();
            base.Load(yamlEvent, state);
            bool flag = false;
            while (!events.Accept<MappingEnd>())
            {
                YamlNode key = ParseNode(events, state);
                YamlNode node2 = ParseNode(events, state);
                try
                {
                    this.children.Add(key, node2);
                }
                catch (ArgumentException exception)
                {
                    throw new YamlException(key.Start, key.End, "Duplicate key", exception);
                }
                flag |= (key is YamlAliasNode) || (node2 is YamlAliasNode);
            }
            if (flag)
            {
                state.AddNodeWithUnresolvedAliases(this);
            }
            events.Expect<MappingEnd>();
        }

        public override void Accept(IYamlVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Add(string key, string value)
        {
            this.children.Add(new YamlScalarNode(key), new YamlScalarNode(value));
        }

        public void Add(string key, YamlNode value)
        {
            this.children.Add(new YamlScalarNode(key), value);
        }

        public void Add(YamlNode key, string value)
        {
            this.children.Add(key, new YamlScalarNode(value));
        }

        public void Add(YamlNode key, YamlNode value)
        {
            this.children.Add(key, value);
        }

        internal override void Emit(IEmitter emitter, EmitterState state)
        {
            emitter.Emit(new MappingStart(base.Anchor, base.Tag, true, this.Style));
            foreach (KeyValuePair<YamlNode, YamlNode> pair in this.children)
            {
                pair.Key.Save(emitter, state);
                pair.Value.Save(emitter, state);
            }
            emitter.Emit(new MappingEnd());
        }

        public override bool Equals(object other)
        {
            bool flag;
            YamlMappingNode node = other as YamlMappingNode;
            if ((node == null) || (!base.Equals(node) || (this.children.Count != node.children.Count)))
            {
                return false;
            }
            using (IEnumerator<KeyValuePair<YamlNode, YamlNode>> enumerator = this.children.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        YamlNode node2;
                        KeyValuePair<YamlNode, YamlNode> current = enumerator.Current;
                        if (node.children.TryGetValue(current.Key, out node2) && SafeEquals(current.Value, node2))
                        {
                            continue;
                        }
                        flag = false;
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
            }
            return flag;
        }

        public IEnumerator<KeyValuePair<YamlNode, YamlNode>> GetEnumerator() => 
            this.children.GetEnumerator();

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            foreach (KeyValuePair<YamlNode, YamlNode> pair in this.children)
            {
                hashCode = CombineHashCodes(CombineHashCodes(hashCode, GetHashCode(pair.Key)), GetHashCode(pair.Value));
            }
            return hashCode;
        }

        internal override void ResolveAliases(DocumentLoadingState state)
        {
            Dictionary<YamlNode, YamlNode> dictionary = null;
            Dictionary<YamlNode, YamlNode> dictionary2 = null;
            foreach (KeyValuePair<YamlNode, YamlNode> pair in this.children)
            {
                if (pair.Key is YamlAliasNode)
                {
                    dictionary = new Dictionary<YamlNode, YamlNode> {
                        { 
                            pair.Key,
                            state.GetNode(pair.Key.Anchor, true, pair.Key.Start, pair.Key.End)
                        }
                    };
                }
                if (pair.Value is YamlAliasNode)
                {
                    dictionary2 = new Dictionary<YamlNode, YamlNode> {
                        { 
                            pair.Key,
                            state.GetNode(pair.Value.Anchor, true, pair.Value.Start, pair.Value.End)
                        }
                    };
                }
            }
            if (dictionary2 != null)
            {
                foreach (KeyValuePair<YamlNode, YamlNode> pair2 in dictionary2)
                {
                    this.children[pair2.Key] = pair2.Value;
                }
            }
            if (dictionary != null)
            {
                foreach (KeyValuePair<YamlNode, YamlNode> pair3 in dictionary)
                {
                    YamlNode node = this.children[pair3.Key];
                    this.children.Remove(pair3.Key);
                    this.children.Add(pair3.Value, node);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("{ ");
            foreach (KeyValuePair<YamlNode, YamlNode> pair in this.children)
            {
                if (builder.Length > 2)
                {
                    builder.Append(", ");
                }
                builder.Append("{ ").Append(pair.Key).Append(", ").Append(pair.Value).Append(" }");
            }
            builder.Append(" }");
            return builder.ToString();
        }

        public IDictionary<YamlNode, YamlNode> Children =>
            this.children;

        public MappingStyle Style { get; set; }

        public override IEnumerable<YamlNode> AllNodes =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        public override YamlNodeType NodeType =>
            YamlNodeType.Mapping;

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<YamlNode>, IEnumerator, IDisposable, IEnumerator<YamlNode>
        {
            internal IEnumerator<KeyValuePair<YamlNode, YamlNode>> $locvar0;
            internal KeyValuePair<YamlNode, YamlNode> <child>__1;
            internal IEnumerator<YamlNode> $locvar1;
            internal YamlNode <node>__2;
            internal IEnumerator<YamlNode> $locvar2;
            internal YamlNode <node>__3;
            internal YamlMappingNode $this;
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
                    case 3:
                        try
                        {
                            switch (num)
                            {
                                case 2:
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
                                    break;

                                case 3:
                                    try
                                    {
                                    }
                                    finally
                                    {
                                        if (this.$locvar2 != null)
                                        {
                                            this.$locvar2.Dispose();
                                        }
                                    }
                                    break;

                                default:
                                    break;
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
                        break;

                    case 1:
                        this.$locvar0 = this.$this.children.GetEnumerator();
                        num = 0xfffffffd;
                        goto TR_0028;

                    case 2:
                    case 3:
                        goto TR_0028;

                    default:
                        goto TR_0000;
                }
                goto TR_0001;
            TR_0000:
                return false;
            TR_0001:
                return true;
            TR_0028:
                try
                {
                    switch (num)
                    {
                        case 2:
                            goto TR_0023;

                        case 3:
                            goto TR_001B;

                        default:
                            break;
                    }
                    goto TR_0026;
                TR_001B:
                    try
                    {
                        switch (num)
                        {
                            default:
                                if (this.$locvar2.MoveNext())
                                {
                                    this.<node>__3 = this.$locvar2.Current;
                                    this.$current = this.<node>__3;
                                    if (!this.$disposing)
                                    {
                                        this.$PC = 3;
                                    }
                                    flag = true;
                                }
                                else
                                {
                                    goto TR_0026;
                                }
                                break;
                        }
                    }
                    finally
                    {
                        if (flag)
                        {
                        }
                        if (this.$locvar2 != null)
                        {
                            this.$locvar2.Dispose();
                        }
                    }
                    goto TR_0001;
                TR_001C:
                    this.$locvar2 = this.<child>__1.Value.AllNodes.GetEnumerator();
                    num = 0xfffffffd;
                    goto TR_001B;
                TR_0023:
                    try
                    {
                        switch (num)
                        {
                            default:
                                if (this.$locvar1.MoveNext())
                                {
                                    this.<node>__2 = this.$locvar1.Current;
                                    this.$current = this.<node>__2;
                                    if (!this.$disposing)
                                    {
                                        this.$PC = 2;
                                    }
                                    flag = true;
                                }
                                else
                                {
                                    goto TR_001C;
                                }
                                break;
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
                    goto TR_0001;
                TR_0026:
                    while (true)
                    {
                        if (this.$locvar0.MoveNext())
                        {
                            this.<child>__1 = this.$locvar0.Current;
                            this.$locvar1 = this.<child>__1.Key.AllNodes.GetEnumerator();
                            num = 0xfffffffd;
                        }
                        else
                        {
                            this.$PC = -1;
                            goto TR_0000;
                        }
                        break;
                    }
                    goto TR_0023;
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
                return new YamlMappingNode.<>c__Iterator0 { $this = this.$this };
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

