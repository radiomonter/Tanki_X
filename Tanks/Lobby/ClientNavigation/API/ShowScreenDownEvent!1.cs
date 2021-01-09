namespace Tanks.Lobby.ClientNavigation.API
{
    using System;

    public class ShowScreenDownEvent<T> : ShowScreenEvent
    {
        public ShowScreenDownEvent() : base(typeof(T), AnimationDirection.DOWN)
        {
        }
    }
}

