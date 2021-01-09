namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x15379996611L)]
    public class RequestSendAgainConfirmationEmailEvent : Event
    {
    }
}

