namespace Tanks.Lobby.ClientNavigation.API
{
    using System;

    public class ShowScreenUpEvent<T> : ShowScreenEvent
    {
        public ShowScreenUpEvent() : base(typeof(T), AnimationDirection.UP)
        {
        }
    }
}

