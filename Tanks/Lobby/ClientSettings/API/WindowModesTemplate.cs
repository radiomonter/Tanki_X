namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d3c2bad25f6c0cL)]
    public interface WindowModesTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        WindowModesComponent windowModes();
    }
}

