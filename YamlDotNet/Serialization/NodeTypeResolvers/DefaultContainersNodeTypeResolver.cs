﻿namespace YamlDotNet.Serialization.NodeTypeResolvers
{
    using System;
    using System.Collections.Generic;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public sealed class DefaultContainersNodeTypeResolver : INodeTypeResolver
    {
        bool INodeTypeResolver.Resolve(NodeEvent nodeEvent, ref Type currentType)
        {
            if (currentType == typeof(object))
            {
                if (nodeEvent is SequenceStart)
                {
                    currentType = typeof(List<object>);
                    return true;
                }
                if (nodeEvent is MappingStart)
                {
                    currentType = typeof(Dictionary<object, object>);
                    return true;
                }
            }
            return false;
        }
    }
}

