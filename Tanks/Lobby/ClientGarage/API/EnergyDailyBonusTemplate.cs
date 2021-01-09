namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15e3d747604L)]
    public interface EnergyDailyBonusTemplate : ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        EnergyBonusComponent energyBonus();
    }
}

