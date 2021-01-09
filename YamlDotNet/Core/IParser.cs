namespace YamlDotNet.Core
{
    using System;
    using YamlDotNet.Core.Events;

    public interface IParser
    {
        bool MoveNext();

        ParsingEvent Current { get; }
    }
}

