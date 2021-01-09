namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x1528213d00dL)]
    public class InvalidRegistrationPasswordEvent : Event
    {
    }
}

