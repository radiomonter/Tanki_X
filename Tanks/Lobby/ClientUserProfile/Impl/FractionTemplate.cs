namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1679b7cb55dL)]
    public interface FractionTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        FractionInfoComponent fractionInfo();
    }
}

