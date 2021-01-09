namespace Tanks.Lobby.ClientNavigation.API
{
    using System;

    public class ShowFirstScreenEvent<T> : ShowFirstScreenEvent
    {
        public ShowFirstScreenEvent() : base(typeof(T))
        {
        }
    }
}

