namespace Tanks.Lobby.ClientNavigation.API
{
    using System;

    public class ShowScreenNoAnimationEvent<T> : ShowScreenEvent
    {
        public ShowScreenNoAnimationEvent() : base(typeof(T), AnimationDirection.NONE)
        {
        }
    }
}

