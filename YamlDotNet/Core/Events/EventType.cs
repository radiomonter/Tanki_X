namespace YamlDotNet.Core.Events
{
    using System;

    internal enum EventType
    {
        None,
        StreamStart,
        StreamEnd,
        DocumentStart,
        DocumentEnd,
        Alias,
        Scalar,
        SequenceStart,
        SequenceEnd,
        MappingStart,
        MappingEnd,
        Comment
    }
}

