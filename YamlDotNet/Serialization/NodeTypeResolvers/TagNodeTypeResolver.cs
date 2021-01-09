namespace YamlDotNet.Serialization.NodeTypeResolvers
{
    using System;
    using System.Collections.Generic;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public sealed class TagNodeTypeResolver : INodeTypeResolver
    {
        private readonly IDictionary<string, Type> tagMappings;

        public TagNodeTypeResolver(IDictionary<string, Type> tagMappings)
        {
            if (tagMappings == null)
            {
                throw new ArgumentNullException("tagMappings");
            }
            this.tagMappings = tagMappings;
        }

        bool INodeTypeResolver.Resolve(NodeEvent nodeEvent, ref Type currentType)
        {
            Type type;
            if (string.IsNullOrEmpty(nodeEvent.Tag) || !this.tagMappings.TryGetValue(nodeEvent.Tag, out type))
            {
                return false;
            }
            currentType = type;
            return true;
        }
    }
}

