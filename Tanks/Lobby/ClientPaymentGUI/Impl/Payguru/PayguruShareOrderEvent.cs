namespace Tanks.Lobby.ClientPaymentGUI.Impl.Payguru
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-1098686186425651898L)]
    public class PayguruShareOrderEvent : Event
    {
        public string Order { get; set; }

        public PayguruAbbreviatedBankInfo[] BanksInfo { get; set; }
    }
}

