namespace YamlDotNet.Serialization.NodeTypeResolvers
{
    using System;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public sealed class TypeNameInTagNodeTypeResolver : INodeTypeResolver
    {
        bool INodeTypeResolver.Resolve(NodeEvent nodeEvent, ref Type currentType)
        {
            if (!string.IsNullOrEmpty(nodeEvent.Tag))
            {
                try
                {
                    currentType = Type.GetType(nodeEvent.Tag.Substring(1), true);
                    return true;
                }
                catch
                {
                }
            }
            return false;
        }
    }
}

