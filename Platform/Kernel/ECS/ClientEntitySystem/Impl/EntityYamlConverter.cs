namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public class EntityYamlConverter : IYamlTypeConverter
    {
        private EngineServiceInternal engine;

        public EntityYamlConverter(EngineServiceInternal engine)
        {
            this.engine = engine;
        }

        public bool Accepts(Type type) => 
            typeof(Entity).IsAssignableFrom(type);

        public object ReadYaml(IParser parser, Type type)
        {
            string path = ((Scalar) parser.Current).Value;
            parser.MoveNext();
            return this.engine.EntityRegistry.GetEntity((long) ConfigurationEntityIdCalculator.Calculate(path));
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}

