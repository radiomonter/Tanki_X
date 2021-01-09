namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public class TemplateDescriptionYamlConverter : IYamlTypeConverter
    {
        private TemplateRegistry templateRegistry;

        public TemplateDescriptionYamlConverter(TemplateRegistry templateRegistry)
        {
            this.templateRegistry = templateRegistry;
        }

        public bool Accepts(Type type) => 
            ReferenceEquals(type, typeof(TemplateDescription));

        public object ReadYaml(IParser parser, Type type)
        {
            long id = long.Parse(((Scalar) parser.Current).Value);
            parser.MoveNext();
            return this.templateRegistry.GetTemplateInfo(id);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}

