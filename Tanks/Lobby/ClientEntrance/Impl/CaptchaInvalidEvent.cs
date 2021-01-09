namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x14fdfa5b0cdL)]
    public class CaptchaInvalidEvent : Event
    {
    }
}

