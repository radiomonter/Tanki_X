namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    public class TDMHUDMessagesComponent : LocalizedControl, Component
    {
        public string MainMessage { get; set; }
    }
}

