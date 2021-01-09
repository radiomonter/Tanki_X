namespace Tanks.Lobby.ClientNavigation.API
{
    using System;

    public class IllegalGoingBackException : Exception
    {
        public IllegalGoingBackException(string message) : base(message)
        {
        }
    }
}

