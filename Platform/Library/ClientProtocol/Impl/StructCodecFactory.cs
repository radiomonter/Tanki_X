namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class StructCodecFactory : CodecFactory
    {
        [CompilerGenerated]
        private static Comparison<PropertyInfo> <>f__am$cache0;

        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs)
        {
            Type structType = !ReflectionUtils.IsNullableType(codecInfoWithAttrs.Info.type) ? codecInfoWithAttrs.Info.type : ReflectionUtils.GetNullableInnerType(codecInfoWithAttrs.Info.type);
            List<PropertyInfo> sortedProperties = GetSortedProperties(structType, protocol);
            List<PropertyRequest> requests = new List<PropertyRequest>(sortedProperties.Count);
            foreach (PropertyInfo info4 in sortedProperties)
            {
                bool optional = Attribute.IsDefined(info4, typeof(ProtocolOptionalAttribute));
                bool varied = Attribute.IsDefined(info4, typeof(ProtocolVariedAttribute));
                CodecInfoWithAttributes codecInfoWithAttributes = new CodecInfoWithAttributes(info4.PropertyType, optional, varied);
                object[] objArray2 = info4.GetCustomAttributes(true);
                int index = 0;
                while (true)
                {
                    if (index >= objArray2.Length)
                    {
                        requests.Add(new PropertyRequest(info4, codecInfoWithAttributes));
                        break;
                    }
                    Attribute attribute = (Attribute) objArray2[index];
                    codecInfoWithAttributes.AddAttribute(attribute);
                    index++;
                }
            }
            return new StructCodec(structType, requests);
        }

        private static int GetOrder(PropertyInfo a)
        {
            int order = 0x7fffffff;
            if (Attribute.IsDefined(a, typeof(ProtocolParameterOrderAttribute)))
            {
                order = ((ProtocolParameterOrderAttribute) Attribute.GetCustomAttribute(a, typeof(ProtocolParameterOrderAttribute))).Order;
            }
            return order;
        }

        private static List<PropertyInfo> GetSortedProperties(Type structType, Protocol protocol)
        {
            List<PropertyInfo> list = new List<PropertyInfo>();
            foreach (PropertyInfo info in structType.GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
            {
                ProtocolTransientAttribute customAttribute = Attribute.GetCustomAttribute(info, typeof(ProtocolTransientAttribute)) as ProtocolTransientAttribute;
                if (((customAttribute == null) ? 0 : customAttribute.MinimalServerProtocolVersion) <= protocol.ServerProtocolVersion)
                {
                    list.Add(info);
                }
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (PropertyInfo a, PropertyInfo b) {
                    int num3 = Math.Sign((int) (GetOrder(a) - GetOrder(b)));
                    return string.CompareOrdinal(a.Name, b.Name);
                };
            }
            list.Sort(<>f__am$cache0);
            return list;
        }
    }
}

