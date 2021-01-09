namespace Tanks.Lobby.ClientGarage.API
{
    using System;

    public interface ITutorialShowStepValidator
    {
        bool ShowAllowed(long stepId);
    }
}

