namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class CartridgeCaseSettingComponent : Component
    {
        public bool DefaultMode;
        public int MaximalCartridgeCount;
    }
}

