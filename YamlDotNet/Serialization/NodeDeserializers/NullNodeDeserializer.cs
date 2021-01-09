namespace YamlDotNet.Serialization.NodeDeserializers
{
    using System;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public sealed class NullNodeDeserializer : INodeDeserializer
    {
        private bool NodeIsNull(NodeEvent nodeEvent)
        {
            if (nodeEvent.Tag == "tag:yaml.org,2002:null")
            {
                return true;
            }
            Scalar scalar = nodeEvent as Scalar;
            if ((scalar == null) || (scalar.Style != ScalarStyle.Plain))
            {
                return false;
            }
            string str = scalar.Value;
            return (((str == string.Empty) || ((str == "~") || ((str == "null") || (str == "Null")))) || (str == "NULL"));
        }

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, out object value)
        {
            value = null;
            NodeEvent nodeEvent = reader.Peek<NodeEvent>();
            bool flag = (nodeEvent != null) && this.NodeIsNull(nodeEvent);
            if (flag)
            {
                reader.SkipThisAndNestedEvents();
            }
            return flag;
        }
    }
}

