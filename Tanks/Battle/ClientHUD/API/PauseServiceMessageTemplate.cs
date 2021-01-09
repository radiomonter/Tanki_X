namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14fd029db29L)]
    public interface PauseServiceMessageTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        PauseMessageComponent pauseMessage();
    }
}

