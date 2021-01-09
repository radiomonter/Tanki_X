namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using YamlDotNet.Core;

    internal class DocumentLoadingState
    {
        private readonly IDictionary<string, YamlNode> anchors = new Dictionary<string, YamlNode>();
        private readonly IList<YamlNode> nodesWithUnresolvedAliases = new List<YamlNode>();

        public void AddAnchor(YamlNode node)
        {
            if (node.Anchor == null)
            {
                throw new ArgumentException("The specified node does not have an anchor");
            }
            if (!this.anchors.ContainsKey(node.Anchor))
            {
                this.anchors.Add(node.Anchor, node);
            }
            else
            {
                object[] args = new object[] { node.Anchor };
                throw new DuplicateAnchorException(node.Start, node.End, string.Format(CultureInfo.InvariantCulture, "The anchor '{0}' already exists", args));
            }
        }

        public void AddNodeWithUnresolvedAliases(YamlNode node)
        {
            this.nodesWithUnresolvedAliases.Add(node);
        }

        public YamlNode GetNode(string anchor, bool throwException, Mark start, Mark end)
        {
            YamlNode node;
            if (this.anchors.TryGetValue(anchor, out node))
            {
                return node;
            }
            if (!throwException)
            {
                return null;
            }
            object[] args = new object[] { anchor };
            throw new AnchorNotFoundException(start, end, string.Format(CultureInfo.InvariantCulture, "The anchor '{0}' does not exists", args));
        }

        public void ResolveAliases()
        {
            foreach (YamlNode node in this.nodesWithUnresolvedAliases)
            {
                node.ResolveAliases(this);
            }
        }
    }
}

