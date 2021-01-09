namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections.Generic;

    public abstract class YamlVisitorBase : IYamlVisitor
    {
        protected YamlVisitorBase()
        {
        }

        protected virtual void Visit(YamlDocument document)
        {
            this.VisitChildren(document);
        }

        protected virtual void Visit(YamlMappingNode mapping)
        {
            this.VisitChildren(mapping);
        }

        protected virtual void Visit(YamlScalarNode scalar)
        {
        }

        protected virtual void Visit(YamlSequenceNode sequence)
        {
            this.VisitChildren(sequence);
        }

        protected virtual void Visit(YamlStream stream)
        {
            this.VisitChildren(stream);
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
                this.VisitPair(pair.Key, pair.Value);
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

        protected virtual void VisitPair(YamlNode key, YamlNode value)
        {
            key.Accept(this);
            value.Accept(this);
        }

        void IYamlVisitor.Visit(YamlDocument document)
        {
            this.Visit(document);
            this.Visited(document);
        }

        void IYamlVisitor.Visit(YamlMappingNode mapping)
        {
            this.Visit(mapping);
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
            this.Visited(sequence);
        }

        void IYamlVisitor.Visit(YamlStream stream)
        {
            this.Visit(stream);
            this.Visited(stream);
        }
    }
}

