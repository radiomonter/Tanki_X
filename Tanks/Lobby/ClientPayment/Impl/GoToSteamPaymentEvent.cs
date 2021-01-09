namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x8d47a0ce2982937L)]
    public class GoToSteamPaymentEvent : Event
    {
    }
}

