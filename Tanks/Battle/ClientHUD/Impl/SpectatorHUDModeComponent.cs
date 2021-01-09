namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SpectatorHUDModeComponent : Component
    {
        public SpectatorHUDModeComponent()
        {
        }

        public SpectatorHUDModeComponent(SpectatorHUDMode hudMode)
        {
            this.HUDMode = hudMode;
        }

        public SpectatorHUDMode HUDMode { get; set; }
    }
}

