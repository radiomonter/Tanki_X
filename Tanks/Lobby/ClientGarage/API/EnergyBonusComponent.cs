namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15e3d747604L)]
    public class EnergyBonusComponent : Component
    {
        public int Bonus { get; set; }

        public int PremiumBonus { get; set; }
    }
}

