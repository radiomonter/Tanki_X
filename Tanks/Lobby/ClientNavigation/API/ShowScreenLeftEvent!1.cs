namespace Tanks.Lobby.ClientNavigation.API
{
    using System;

    public class ShowScreenLeftEvent<T> : ShowScreenEvent
    {
        public ShowScreenLeftEvent() : base(typeof(T), AnimationDirection.LEFT)
        {
        }
    }
}

