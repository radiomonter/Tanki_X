namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    public class HelpButtonComponent : LocalizedControl, Component
    {
        public string Url { get; set; }
    }
}

