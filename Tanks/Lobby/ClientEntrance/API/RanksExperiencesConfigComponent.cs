namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class RanksExperiencesConfigComponent : Component
    {
        public int[] RanksExperiences { get; set; }
    }
}

