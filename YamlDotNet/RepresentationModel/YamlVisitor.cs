namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections.Generic;

    [Obsolete("Use YamlVisitorBase")]
    public abstract class YamlVisitor : IYamlVisitor
    {
        protected YamlVisitor()
        {
        }

        protected virtual void Visit(YamlDocument document)
        {
        }

        protected virtual void Visit(YamlMappingNode mapping)
        {
        }

        protected virtual void Visit(YamlScalarNode scalar)
        {
        }

        protected virtual void Visit(YamlSequenceNode sequence)
        {
        }

        protected virtual void Visit(YamlStream stream)
        {
        }

        protected virtual void VisitChildren(YamlDocument document)
        {
            if (document.RootNode != null)
            {
                document.RootNode.Accept(this);
            }
        }

        protected virtual void VisitChildren(YamlMappingNode mapping)
        {
            foreach (KeyValuePair<YamlNode, YamlNode> pair in mapping.Children)
            {
                pair.Key.Accept(this);
                pair.Value.Accept(this);
            }
        }

        protected virtual void VisitChildren(YamlSequenceNode sequence)
        {
            foreach (YamlNode node in sequence.Children)
            {
                node.Accept(this);
            }
        }

        protected virtual void VisitChildren(YamlStream stream)
        {
            foreach (YamlDocument document in stream.Documents)
            {
                document.Accept(this);
            }
        }

        protected virtual void Visited(YamlDocument document)
        {
        }

        protected virtual void Visited(YamlMappingNode mapping)
        {
        }

        protected virtual void Visited(YamlScalarNode scalar)
        {
        }

        protected virtual void Visited(YamlSequenceNode sequence)
        {
        }

        protected virtual void Visited(YamlStream stream)
        {
        }

        void IYamlVisitor.Visit(YamlDocument document)
        {
            this.Visit(document);
            this.VisitChildren(document);
            this.Visited(document);
        }

        void IYamlVisitor.Visit(YamlMappingNode mapping)
        {
            this.Visit(mapping);
            this.VisitChildren(mapping);
            this.Visited(mapping);
        }

        void IYamlVisitor.Visit(YamlScalarNode scalar)
        {
            this.Visit(scalar);
            this.Visited(scalar);
        }

        void IYamlVisitor.Visit(YamlSequenceNode sequence)
        {
            this.Visit(sequence);
            this.VisitChildren(sequence);
            this.Visited(sequence);
        }

        void IYamlVisitor.Visit(YamlStream stream)
        {
            this.Visit(stream);
            this.VisitChildren(stream);
            this.Visited(stream);
        }
    }
}

