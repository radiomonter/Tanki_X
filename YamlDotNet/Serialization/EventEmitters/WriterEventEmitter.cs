namespace YamlDotNet.Serialization.EventEmitters
{
    using System;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public sealed class WriterEventEmitter : IEventEmitter
    {
        private readonly IEmitter emitter;

        public WriterEventEmitter(IEmitter emitter)
        {
            this.emitter = emitter;
        }

        void IEventEmitter.Emit(AliasEventInfo eventInfo)
        {
            this.emitter.Emit(new AnchorAlias(eventInfo.Alias));
        }

        void IEventEmitter.Emit(MappingEndEventInfo eventInfo)
        {
            this.emitter.Emit(new MappingEnd());
        }

        void IEventEmitter.Emit(MappingStartEventInfo eventInfo)
        {
            this.emitter.Emit(new MappingStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
        }

        void IEventEmitter.Emit(ScalarEventInfo eventInfo)
        {
            this.emitter.Emit(new Scalar(eventInfo.Anchor, eventInfo.Tag, eventInfo.RenderedValue, eventInfo.Style, eventInfo.IsPlainImplicit, eventInfo.IsQuotedImplicit));
        }

        void IEventEmitter.Emit(SequenceEndEventInfo eventInfo)
        {
            this.emitter.Emit(new SequenceEnd());
        }

        void IEventEmitter.Emit(SequenceStartEventInfo eventInfo)
        {
            this.emitter.Emit(new SequenceStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
        }
    }
}

