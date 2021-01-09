namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x14ecfac1276L)]
    public class LoginFailedEvent : Event
    {
    }
}

