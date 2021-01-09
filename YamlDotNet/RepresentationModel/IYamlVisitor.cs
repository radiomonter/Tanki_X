namespace YamlDotNet.RepresentationModel
{
    using System;

    public interface IYamlVisitor
    {
        void Visit(YamlDocument document);
        void Visit(YamlMappingNode mapping);
        void Visit(YamlScalarNode scalar);
        void Visit(YamlSequenceNode sequence);
        void Visit(YamlStream stream);
    }
}

