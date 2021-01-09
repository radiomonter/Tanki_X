namespace YamlDotNet.Serialization.Converters
{
    using System;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public class GuidConverter : IYamlTypeConverter
    {
        private readonly bool jsonCompatible;

        public GuidConverter(bool jsonCompatible)
        {
            this.jsonCompatible = jsonCompatible;
        }

        public bool Accepts(Type type) => 
            ReferenceEquals(type, typeof(Guid));

        public object ReadYaml(IParser parser, Type type)
        {
            string g = ((Scalar) parser.Current).Value;
            parser.MoveNext();
            return new Guid(g);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            Guid guid = (Guid) value;
            emitter.Emit(new Scalar(null, null, guid.ToString("D"), !this.jsonCompatible ? ScalarStyle.Any : ScalarStyle.DoubleQuoted, true, false));
        }
    }
}

