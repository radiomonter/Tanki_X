namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x151f2179cadL)]
    public interface BattleChatHUDTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        BattleChatLocalizedStringsComponent battleChatLocalizedStrings();
    }
}

