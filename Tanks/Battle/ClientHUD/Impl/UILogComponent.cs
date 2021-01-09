namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    public class UILogComponent : Component
    {
        public UILogComponent(Tanks.Lobby.ClientControls.API.UILog uiLog)
        {
            this.UILog = uiLog;
        }

        public Tanks.Lobby.ClientControls.API.UILog UILog { get; set; }
    }
}

