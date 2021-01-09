namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x151ec5188ceL)]
    public class UserProfileLoadedEvent : Event
    {
    }
}

