namespace YamlDotNet.Serialization
{
    using System;
    using YamlDotNet.Core.Events;

    public interface INodeTypeResolver
    {
        bool Resolve(NodeEvent nodeEvent, ref Type currentType);
    }
}

