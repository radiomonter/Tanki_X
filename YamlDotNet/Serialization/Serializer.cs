namespace YamlDotNet.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization.EventEmitters;
    using YamlDotNet.Serialization.NamingConventions;
    using YamlDotNet.Serialization.ObjectGraphTraversalStrategies;
    using YamlDotNet.Serialization.ObjectGraphVisitors;
    using YamlDotNet.Serialization.TypeInspectors;
    using YamlDotNet.Serialization.TypeResolvers;
    using YamlDotNet.Serialization.Utilities;

    public sealed class Serializer
    {
        private readonly SerializationOptions options;
        private readonly INamingConvention namingConvention;
        private readonly ITypeResolver typeResolver;
        private readonly YamlAttributeOverrides overrides;

        public Serializer(SerializationOptions options = 0, INamingConvention namingConvention = null, YamlAttributeOverrides overrides = null)
        {
            this.options = options;
            INamingConvention convention1 = namingConvention;
            if (namingConvention == null)
            {
                INamingConvention local1 = namingConvention;
                convention1 = new NullNamingConvention();
            }
            this.namingConvention = convention1;
            this.overrides = overrides;
            this.Converters = new List<IYamlTypeConverter>();
            foreach (IYamlTypeConverter converter in YamlTypeConverters.GetBuiltInConverters(this.IsOptionSet(SerializationOptions.JsonCompatible)))
            {
                this.Converters.Add(converter);
            }
            this.typeResolver = !this.IsOptionSet(SerializationOptions.DefaultToStaticType) ? ((ITypeResolver) new DynamicTypeResolver()) : ((ITypeResolver) new StaticTypeResolver());
        }

        private IObjectGraphVisitor CreateEmittingVisitor(IEmitter emitter, IObjectGraphTraversalStrategy traversalStrategy, IEventEmitter eventEmitter, IObjectDescriptor graph)
        {
            IObjectGraphVisitor nextVisitor = new EmittingObjectGraphVisitor(eventEmitter);
            nextVisitor = new CustomSerializationObjectGraphVisitor(emitter, nextVisitor, this.Converters);
            if (!this.IsOptionSet(SerializationOptions.DisableAliases))
            {
                AnchorAssigner visitor = new AnchorAssigner();
                traversalStrategy.Traverse(graph, visitor);
                nextVisitor = new AnchorAssigningObjectGraphVisitor(nextVisitor, eventEmitter, visitor);
            }
            if (!this.IsOptionSet(SerializationOptions.EmitDefaults))
            {
                nextVisitor = new DefaultExclusiveObjectGraphVisitor(nextVisitor);
            }
            return nextVisitor;
        }

        private IEventEmitter CreateEventEmitter(IEmitter emitter)
        {
            WriterEventEmitter nextEmitter = new WriterEventEmitter(emitter);
            return (!this.IsOptionSet(SerializationOptions.JsonCompatible) ? ((IEventEmitter) new TypeAssigningEventEmitter(nextEmitter, this.IsOptionSet(SerializationOptions.Roundtrip))) : ((IEventEmitter) new JsonEventEmitter(nextEmitter)));
        }

        private IObjectGraphTraversalStrategy CreateTraversalStrategy()
        {
            ITypeInspector innerTypeDescriptor = new ReadablePropertiesTypeInspector(this.typeResolver);
            if (this.IsOptionSet(SerializationOptions.Roundtrip))
            {
                innerTypeDescriptor = new ReadableAndWritablePropertiesTypeInspector(innerTypeDescriptor);
            }
            innerTypeDescriptor = new YamlAttributesTypeInspector(new YamlAttributeOverridesInspector(new NamingConventionTypeInspector(innerTypeDescriptor, this.namingConvention), this.overrides));
            if (this.IsOptionSet(SerializationOptions.DefaultToStaticType))
            {
                innerTypeDescriptor = new CachedTypeInspector(innerTypeDescriptor);
            }
            return (!this.IsOptionSet(SerializationOptions.Roundtrip) ? new FullObjectGraphTraversalStrategy(this, innerTypeDescriptor, this.typeResolver, 50, this.namingConvention) : new RoundtripObjectGraphTraversalStrategy(this, innerTypeDescriptor, this.typeResolver, 50));
        }

        private void EmitDocument(IEmitter emitter, IObjectDescriptor graph)
        {
            IObjectGraphTraversalStrategy traversalStrategy = this.CreateTraversalStrategy();
            IObjectGraphVisitor visitor = this.CreateEmittingVisitor(emitter, traversalStrategy, this.CreateEventEmitter(emitter), graph);
            emitter.Emit(new StreamStart());
            emitter.Emit(new DocumentStart());
            traversalStrategy.Traverse(graph, visitor);
            emitter.Emit(new DocumentEnd(true));
            emitter.Emit(new StreamEnd());
        }

        private bool IsOptionSet(SerializationOptions option) => 
            (this.options & option) != SerializationOptions.None;

        public void RegisterTypeConverter(IYamlTypeConverter converter)
        {
            this.Converters.Insert(0, converter);
        }

        public void Serialize(TextWriter writer, object graph)
        {
            this.Serialize(new Emitter(writer), graph);
        }

        public void Serialize(IEmitter emitter, object graph)
        {
            if (emitter == null)
            {
                throw new ArgumentNullException("emitter");
            }
            this.EmitDocument(emitter, new ObjectDescriptor(graph, (graph == null) ? typeof(object) : graph.GetType(), typeof(object)));
        }

        public void Serialize(TextWriter writer, object graph, Type type)
        {
            this.Serialize(new Emitter(writer), graph, type);
        }

        public void Serialize(IEmitter emitter, object graph, Type type)
        {
            if (emitter == null)
            {
                throw new ArgumentNullException("emitter");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            this.EmitDocument(emitter, new ObjectDescriptor(graph, type, type));
        }

        internal IList<IYamlTypeConverter> Converters { get; private set; }
    }
}

