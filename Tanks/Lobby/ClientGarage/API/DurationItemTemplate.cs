namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x16068c216d7L)]
    public interface DurationItemTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        DurationItemComponent durationItem();
    }
}

