namespace YamlDotNet.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using YamlDotNet.Core.Events;

    public class EventReader
    {
        private readonly IParser parser;
        private bool endOfStream;

        public EventReader(IParser parser)
        {
            this.parser = parser;
            this.MoveNext();
        }

        public bool Accept<T>() where T: ParsingEvent
        {
            this.ThrowIfAtEndOfStream();
            return (this.parser.Current is T);
        }

        public T Allow<T>() where T: ParsingEvent
        {
            if (!this.Accept<T>())
            {
                return null;
            }
            T current = (T) this.parser.Current;
            this.MoveNext();
            return current;
        }

        public T Expect<T>() where T: ParsingEvent
        {
            T local = this.Allow<T>();
            if (local != null)
            {
                return local;
            }
            ParsingEvent current = this.parser.Current;
            object[] args = new object[] { typeof(T).Name, current.GetType().Name, current.Start };
            throw new YamlException(current.Start, current.End, string.Format(CultureInfo.InvariantCulture, "Expected '{0}', got '{1}' (at {2}).", args));
        }

        private void MoveNext()
        {
            this.endOfStream = !this.parser.MoveNext();
        }

        public T Peek<T>() where T: ParsingEvent => 
            this.Accept<T>() ? ((T) this.parser.Current) : null;

        public void SkipThisAndNestedEvents()
        {
            int num = 0;
            while (true)
            {
                num += this.Peek<ParsingEvent>().NestingIncrease;
                this.MoveNext();
                if (num <= 0)
                {
                    return;
                }
            }
        }

        private void ThrowIfAtEndOfStream()
        {
            if (this.endOfStream)
            {
                throw new EndOfStreamException();
            }
        }

        public IParser Parser =>
            this.parser;
    }
}

