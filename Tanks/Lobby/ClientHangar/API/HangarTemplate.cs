namespace Tanks.Lobby.ClientHangar.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientHangar.Impl;

    [SerialVersionUID(0x14e77bcfc49L)]
    public interface HangarTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        HangarAssetComponent hangarAsset();
    }
}

