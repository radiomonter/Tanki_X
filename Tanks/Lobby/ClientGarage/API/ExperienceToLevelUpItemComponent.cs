namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x14f069ccb28L), Shared]
    public class ExperienceToLevelUpItemComponent : Component
    {
        public int RemainingExperience { get; set; }

        public int InitLevelExperience { get; set; }

        public int FinalLevelExperience { get; set; }
    }
}

