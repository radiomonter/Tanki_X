namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ff9b336e8L)]
    public interface AutokickServiceMessageTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        AutokickMessageComponent autokickMessage();
    }
}

