namespace YamlDotNet.Serialization
{
    using System;
    using System.Collections.Generic;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;

    public sealed class StreamFragment : IYamlSerializable
    {
        private readonly List<ParsingEvent> events = new List<ParsingEvent>();

        void IYamlSerializable.ReadYaml(IParser parser)
        {
            this.events.Clear();
            int num = 0;
            while (parser.MoveNext())
            {
                this.events.Add(parser.Current);
                num += parser.Current.NestingIncrease;
                if (num <= 0)
                {
                    return;
                }
            }
            throw new InvalidOperationException("The parser has reached the end before deserialization completed.");
        }

        void IYamlSerializable.WriteYaml(IEmitter emitter)
        {
            foreach (ParsingEvent event2 in this.events)
            {
                emitter.Emit(event2);
            }
        }

        public IList<ParsingEvent> Events =>
            this.events;
    }
}

