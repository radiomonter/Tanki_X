namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4f078ae8d3996L)]
    public interface CardTierTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CardTierComponent cardTier();
    }
}

