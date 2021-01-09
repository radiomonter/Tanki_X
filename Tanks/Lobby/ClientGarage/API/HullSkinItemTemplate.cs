namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1562b762509L)]
    public interface HullSkinItemTemplate : Template
    {
        [AutoAdded]
        HullSkinItemComponent hullSkinItem();
    }
}

