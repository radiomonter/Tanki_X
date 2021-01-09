namespace Platform.Library.ClientUnityIntegration.Impl
{
    using System;
    using System.Globalization;
    using UnityEngine;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public class Vector3YamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => 
            typeof(Vector3).IsAssignableFrom(type);

        private static Vector3 ReadField(EventReader eventReader, Vector3 v)
        {
            Scalar scalar = eventReader.Expect<Scalar>();
            float num = float.Parse(eventReader.Expect<Scalar>().Value);
            if (scalar.Value == "x")
            {
                v.x = num;
            }
            if (scalar.Value == "y")
            {
                v.y = num;
            }
            if (scalar.Value == "z")
            {
                v.z = num;
            }
            return v;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            EventReader eventReader = new EventReader(parser);
            eventReader.Expect<MappingStart>();
            Vector3 v = new Vector3();
            v = ReadField(eventReader, ReadField(eventReader, ReadField(eventReader, v)));
            eventReader.Expect<MappingEnd>();
            return v;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            Vector3 vector = (Vector3) value;
            emitter.Emit(new MappingStart());
            emitter.Emit(new Scalar("x"));
            emitter.Emit(new Scalar(vector.x.ToString(CultureInfo.InvariantCulture)));
            emitter.Emit(new Scalar("y"));
            emitter.Emit(new Scalar(vector.y.ToString(CultureInfo.InvariantCulture)));
            emitter.Emit(new Scalar("z"));
            emitter.Emit(new Scalar(vector.z.ToString(CultureInfo.InvariantCulture)));
            emitter.Emit(new MappingEnd());
        }
    }
}

