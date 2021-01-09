namespace YamlDotNet.Core
{
    using System;
    using YamlDotNet.Core.Events;

    public interface IEmitter
    {
        void Emit(ParsingEvent @event);
    }
}

