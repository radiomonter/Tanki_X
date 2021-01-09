namespace YamlDotNet.Serialization
{
    using System;

    public interface IObjectGraphTraversalStrategy
    {
        void Traverse(IObjectDescriptor graph, IObjectGraphVisitor visitor);
    }
}

