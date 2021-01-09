namespace Tanks.Lobby.ClientControls.API
{
    using System;

    public interface UILog
    {
        void AddMessage(string messageText);
        void Clear();
    }
}

