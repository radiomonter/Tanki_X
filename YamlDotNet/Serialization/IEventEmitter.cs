namespace YamlDotNet.Serialization
{
    using System;

    public interface IEventEmitter
    {
        void Emit(AliasEventInfo eventInfo);
        void Emit(MappingEndEventInfo eventInfo);
        void Emit(MappingStartEventInfo eventInfo);
        void Emit(ScalarEventInfo eventInfo);
        void Emit(SequenceEndEventInfo eventInfo);
        void Emit(SequenceStartEventInfo eventInfo);
    }
}

