namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1523ff6dbecL)]
    public interface RanksNamesTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        RanksNamesComponent ranksNames();
    }
}

