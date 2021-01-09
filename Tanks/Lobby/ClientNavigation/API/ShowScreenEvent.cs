namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ShowScreenEvent : Event
    {
        public ShowScreenEvent(Type screenType, AnimationDirection animationDirection)
        {
            this.ShowScreenData = new Tanks.Lobby.ClientNavigation.API.ShowScreenData(screenType, animationDirection);
        }

        public void SetContext(Entity context, bool autoDelete)
        {
            this.ShowScreenData.Context = context;
            this.ShowScreenData.AutoDeleteContext = autoDelete;
        }

        public Tanks.Lobby.ClientNavigation.API.ShowScreenData ShowScreenData { get; protected set; }
    }
}

