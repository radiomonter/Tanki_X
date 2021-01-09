namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d4c53fa196b8c9L)]
    public class ModuleUpgradePropertiesInfoComponent : Component
    {
        public TemplateDescription Template { get; set; }

        public string Path { get; set; }
    }
}

