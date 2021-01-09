namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14fd0375f62L)]
    public interface SelfDestructionServiceMessageTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        SelfDestructionMessageComponent selfDestructionMessage();
    }
}

