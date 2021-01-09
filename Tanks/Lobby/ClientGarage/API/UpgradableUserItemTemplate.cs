namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14e6cc0fa7aL)]
    public interface UpgradableUserItemTemplate : UserItemTemplate, Template
    {
        ExperienceToLevelUpItemComponent experienceToLevelUpItem();
        UpgradeLevelItemComponent upgradeLevelItem();
        UpgradeMaxLevelItemComponent upgradeMaxLevelItem();
    }
}

