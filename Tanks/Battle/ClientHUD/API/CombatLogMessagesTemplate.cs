namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d2879cda9aed27L)]
    public interface CombatLogMessagesTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CombatLogCommonMessagesComponent combatLogCommonMessages();
        [AutoAdded, PersistentConfig("", false)]
        CombatLogCTFMessagesComponent combatLogCtfMessages();
        [AutoAdded, PersistentConfig("", false)]
        CombatLogDMMessagesComponent combatLogDMMessages();
        [AutoAdded, PersistentConfig("", false)]
        CombatLogTDMMessagesComponent combatLogTDMMessages();
    }
}

