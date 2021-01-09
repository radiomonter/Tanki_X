namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class OverallChannelComponent : Component
    {
        public Tanks.Lobby.ClientCommunicator.Impl.ChatType ChatType =>
            Tanks.Lobby.ClientCommunicator.Impl.ChatType.OVERALL;
    }
}

