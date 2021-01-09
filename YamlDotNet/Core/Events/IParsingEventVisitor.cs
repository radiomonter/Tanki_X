namespace YamlDotNet.Core.Events
{
    using System;

    public interface IParsingEventVisitor
    {
        void Visit(AnchorAlias e);
        void Visit(Comment e);
        void Visit(DocumentEnd e);
        void Visit(DocumentStart e);
        void Visit(MappingEnd e);
        void Visit(MappingStart e);
        void Visit(Scalar e);
        void Visit(SequenceEnd e);
        void Visit(SequenceStart e);
        void Visit(StreamEnd e);
        void Visit(StreamStart e);
    }
}

