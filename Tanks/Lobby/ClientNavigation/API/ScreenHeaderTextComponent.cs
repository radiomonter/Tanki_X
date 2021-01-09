namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ScreenHeaderTextComponent : Component
    {
        public ScreenHeaderTextComponent()
        {
        }

        public ScreenHeaderTextComponent(string headerText)
        {
            this.HeaderText = headerText;
        }

        public string HeaderText { get; set; }
    }
}

