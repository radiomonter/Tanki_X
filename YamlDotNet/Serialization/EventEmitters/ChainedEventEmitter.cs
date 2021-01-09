namespace YamlDotNet.Serialization.EventEmitters
{
    using System;
    using YamlDotNet.Serialization;

    public abstract class ChainedEventEmitter : IEventEmitter
    {
        protected readonly IEventEmitter nextEmitter;

        protected ChainedEventEmitter(IEventEmitter nextEmitter)
        {
            if (nextEmitter == null)
            {
                throw new ArgumentNullException("nextEmitter");
            }
            this.nextEmitter = nextEmitter;
        }

        public virtual void Emit(AliasEventInfo eventInfo)
        {
            this.nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(MappingEndEventInfo eventInfo)
        {
            this.nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(MappingStartEventInfo eventInfo)
        {
            this.nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(ScalarEventInfo eventInfo)
        {
            this.nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(SequenceEndEventInfo eventInfo)
        {
            this.nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(SequenceStartEventInfo eventInfo)
        {
            this.nextEmitter.Emit(eventInfo);
        }
    }
}

