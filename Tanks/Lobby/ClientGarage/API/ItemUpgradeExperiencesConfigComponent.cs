namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x157dc11a90fL)]
    public class ItemUpgradeExperiencesConfigComponent : Component
    {
        public int[] LevelsExperiences { get; set; }
    }
}

