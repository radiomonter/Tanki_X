namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x15fbf19ac7bL)]
    public class RequestToSquadCanceledEvent : Event
    {
    }
}

