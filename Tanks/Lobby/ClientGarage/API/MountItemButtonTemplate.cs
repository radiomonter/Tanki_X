namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x1504b45dd31L)]
    public interface MountItemButtonTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        MountItemButtonTextComponent mountItemButtonText();
    }
}

