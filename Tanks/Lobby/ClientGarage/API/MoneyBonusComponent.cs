namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x16194b579c8L)]
    public class MoneyBonusComponent : Component
    {
        public int Bonus { get; set; }
    }
}

