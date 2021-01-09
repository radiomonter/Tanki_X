namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;

    [Serializable]
    public class YamlDocument
    {
        public YamlDocument(string rootNode)
        {
            this.RootNode = new YamlScalarNode(rootNode);
        }

        internal YamlDocument(EventReader events)
        {
            DocumentLoadingState state = new DocumentLoadingState();
            events.Expect<DocumentStart>();
            while (!events.Accept<DocumentEnd>())
            {
                this.RootNode = YamlNode.ParseNode(events, state);
                if (this.RootNode is YamlAliasNode)
                {
                    throw new YamlException();
                }
            }
            state.ResolveAliases();
            events.Expect<DocumentEnd>();
        }

        public YamlDocument(YamlNode rootNode)
        {
            this.RootNode = rootNode;
        }

        public void Accept(IYamlVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void AssignAnchors()
        {
            new AnchorAssigningVisitor().AssignAnchors(this);
        }

        internal void Save(IEmitter emitter, bool assignAnchors = true)
        {
            if (assignAnchors)
            {
                this.AssignAnchors();
            }
            emitter.Emit(new DocumentStart());
            this.RootNode.Save(emitter, new EmitterState());
            emitter.Emit(new DocumentEnd(false));
        }

        public YamlNode RootNode { get; private set; }

        public IEnumerable<YamlNode> AllNodes =>
            this.RootNode.AllNodes;

        private class AnchorAssigningVisitor : YamlVisitor
        {
            private readonly HashSet<string> existingAnchors = new HashSet<string>();
            private readonly Dictionary<YamlNode, bool> visitedNodes = new Dictionary<YamlNode, bool>();

            public void AssignAnchors(YamlDocument document)
            {
                this.existingAnchors.Clear();
                this.visitedNodes.Clear();
                document.Accept(this);
                Random random = new Random();
                foreach (KeyValuePair<YamlNode, bool> pair in this.visitedNodes)
                {
                    if (pair.Value)
                    {
                        while (true)
                        {
                            string item = random.Next().ToString(CultureInfo.InvariantCulture);
                            if (!this.existingAnchors.Contains(item))
                            {
                                this.existingAnchors.Add(item);
                                pair.Key.Anchor = item;
                                break;
                            }
                        }
                    }
                }
            }

            protected override void Visit(YamlMappingNode mapping)
            {
                this.VisitNode(mapping);
            }

            protected override void Visit(YamlScalarNode scalar)
            {
                this.VisitNode(scalar);
            }

            protected override void Visit(YamlSequenceNode sequence)
            {
                this.VisitNode(sequence);
            }

            private void VisitNode(YamlNode node)
            {
                if (!string.IsNullOrEmpty(node.Anchor))
                {
                    this.existingAnchors.Add(node.Anchor);
                }
                else
                {
                    bool flag;
                    if (!this.visitedNodes.TryGetValue(node, out flag))
                    {
                        this.visitedNodes.Add(node, false);
                    }
                    else if (!flag)
                    {
                        this.visitedNodes[node] = true;
                    }
                }
            }
        }
    }
}

