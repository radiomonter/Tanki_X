namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x15281c5a924L)]
    public class ProceedToExternalPaymentEvent : ProcessPaymentEvent
    {
    }
}

