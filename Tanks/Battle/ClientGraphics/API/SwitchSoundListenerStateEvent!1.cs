namespace Tanks.Battle.ClientGraphics.API
{
    using System;

    public class SwitchSoundListenerStateEvent<T> : SwitchSoundListenerStateEvent where T: Node
    {
        public SwitchSoundListenerStateEvent() : base(typeof(T))
        {
        }
    }
}

