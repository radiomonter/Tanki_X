namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization;

    public sealed class CustomSerializationObjectGraphVisitor : ChainedObjectGraphVisitor
    {
        private readonly IEmitter emitter;
        private readonly IEnumerable<IYamlTypeConverter> typeConverters;

        public CustomSerializationObjectGraphVisitor(IEmitter emitter, IObjectGraphVisitor nextVisitor, IEnumerable<IYamlTypeConverter> typeConverters) : base(nextVisitor)
        {
            this.emitter = emitter;
            this.typeConverters = (typeConverters == null) ? Enumerable.Empty<IYamlTypeConverter>() : typeConverters.ToList<IYamlTypeConverter>();
        }

        public override bool Enter(IObjectDescriptor value)
        {
            <Enter>c__AnonStorey0 storey = new <Enter>c__AnonStorey0 {
                value = value
            };
            IYamlTypeConverter converter = this.typeConverters.FirstOrDefault<IYamlTypeConverter>(new Func<IYamlTypeConverter, bool>(storey.<>m__0));
            if (converter != null)
            {
                converter.WriteYaml(this.emitter, storey.value.Value, storey.value.Type);
                return false;
            }
            IYamlSerializable serializable = storey.value as IYamlSerializable;
            if (serializable == null)
            {
                return base.Enter(storey.value);
            }
            serializable.WriteYaml(this.emitter);
            return false;
        }

        [CompilerGenerated]
        private sealed class <Enter>c__AnonStorey0
        {
            internal IObjectDescriptor value;

            internal bool <>m__0(IYamlTypeConverter t) => 
                t.Accepts(this.value.Type);
        }
    }
}

