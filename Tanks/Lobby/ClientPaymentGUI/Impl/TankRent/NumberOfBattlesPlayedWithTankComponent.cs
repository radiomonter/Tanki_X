namespace Tanks.Lobby.ClientPaymentGUI.Impl.TankRent
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x160552dc6aeL)]
    public class NumberOfBattlesPlayedWithTankComponent : SharedChangeableComponent
    {
        public int BattlesLeft { get; set; }
    }
}

