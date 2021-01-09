namespace Tanks.Lobby.ClientNavigation.API
{
    using System;

    public class ShowFirstScreenEvent : ShowScreenEvent
    {
        public ShowFirstScreenEvent(Type screenType) : base(screenType, AnimationDirection.NONE)
        {
        }
    }
}

