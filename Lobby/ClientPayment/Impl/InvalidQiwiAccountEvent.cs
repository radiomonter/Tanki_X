namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x158d7e4167fL)]
    public class InvalidQiwiAccountEvent : Event
    {
    }
}

