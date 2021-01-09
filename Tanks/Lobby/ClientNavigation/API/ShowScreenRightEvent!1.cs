namespace Tanks.Lobby.ClientNavigation.API
{
    using System;

    public class ShowScreenRightEvent<T> : ShowScreenEvent
    {
        public ShowScreenRightEvent() : base(typeof(T), AnimationDirection.RIGHT)
        {
        }
    }
}

