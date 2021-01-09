namespace YamlDotNet.RepresentationModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;

    [Serializable]
    public class YamlStream : IEnumerable<YamlDocument>, IEnumerable
    {
        private readonly IList<YamlDocument> documents;

        public YamlStream()
        {
            this.documents = new List<YamlDocument>();
        }

        public YamlStream(IEnumerable<YamlDocument> documents)
        {
            this.documents = new List<YamlDocument>();
            foreach (YamlDocument document in documents)
            {
                this.documents.Add(document);
            }
        }

        public YamlStream(params YamlDocument[] documents) : this((IEnumerable<YamlDocument>) documents)
        {
        }

        public void Accept(IYamlVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Add(YamlDocument document)
        {
            this.documents.Add(document);
        }

        public IEnumerator<YamlDocument> GetEnumerator() => 
            this.documents.GetEnumerator();

        public void Load(TextReader input)
        {
            this.Load(new EventReader(new Parser(input)));
        }

        public void Load(EventReader reader)
        {
            this.documents.Clear();
            reader.Expect<StreamStart>();
            while (!reader.Accept<StreamEnd>())
            {
                YamlDocument item = new YamlDocument(reader);
                this.documents.Add(item);
            }
            reader.Expect<StreamEnd>();
        }

        public void Save(TextWriter output)
        {
            this.Save(output, true);
        }

        public void Save(TextWriter output, bool assignAnchors)
        {
            IEmitter emitter = new Emitter(output);
            emitter.Emit(new StreamStart());
            foreach (YamlDocument document in this.documents)
            {
                document.Save(emitter, assignAnchors);
            }
            emitter.Emit(new StreamEnd());
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public IList<YamlDocument> Documents =>
            this.documents;
    }
}

