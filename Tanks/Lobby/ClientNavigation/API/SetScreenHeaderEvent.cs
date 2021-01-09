namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class SetScreenHeaderEvent : Event
    {
        private string header;
        private bool animate = true;

        public void Animated(string header)
        {
            this.Animate = true;
            this.Header = header;
        }

        public void Immediate(string header)
        {
            this.Animate = false;
            this.Header = header;
        }

        public string Header
        {
            get => 
                this.header ?? string.Empty;
            set => 
                this.header = value;
        }

        public bool Animate
        {
            get => 
                this.animate;
            set => 
                this.animate = value;
        }
    }
}

