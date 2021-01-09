namespace YamlDotNet.Serialization.ObjectGraphTraversalStrategies
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using YamlDotNet;
    using YamlDotNet.Serialization;

    public class RoundtripObjectGraphTraversalStrategy : FullObjectGraphTraversalStrategy
    {
        public RoundtripObjectGraphTraversalStrategy(Serializer serializer, ITypeInspector typeDescriptor, ITypeResolver typeResolver, int maxRecursion) : base(serializer, typeDescriptor, typeResolver, maxRecursion, null)
        {
        }

        protected override void TraverseProperties(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth)
        {
            <TraverseProperties>c__AnonStorey0 storey = new <TraverseProperties>c__AnonStorey0 {
                value = value
            };
            if (storey.value.Type.HasDefaultConstructor() || base.serializer.Converters.Any<IYamlTypeConverter>(new Func<IYamlTypeConverter, bool>(storey.<>m__0)))
            {
                base.TraverseProperties(storey.value, visitor, currentDepth);
            }
            else
            {
                object[] args = new object[] { storey.value.Type };
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' cannot be deserialized because it does not have a default constructor or a type converter.", args));
            }
        }

        [CompilerGenerated]
        private sealed class <TraverseProperties>c__AnonStorey0
        {
            internal IObjectDescriptor value;

            internal bool <>m__0(IYamlTypeConverter c) => 
                c.Accepts(this.value.Type);
        }
    }
}

