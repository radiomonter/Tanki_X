namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class GoBackSoundEffectComponent : UISoundEffectController, Component
    {
        public override string HandlerName =>
            "Cancel";
    }
}

