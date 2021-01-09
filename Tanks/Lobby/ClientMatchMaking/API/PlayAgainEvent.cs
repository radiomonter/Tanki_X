namespace Tanks.Lobby.ClientMatchMaking.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PlayAgainEvent : Event
    {
        private bool modeIsAvailable;

        public bool ModeIsAvailable
        {
            get => 
                this.modeIsAvailable;
            set => 
                this.modeIsAvailable = value;
        }

        public Entity MatchMackingMode { get; set; }
    }
}

