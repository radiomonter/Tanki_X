namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x16194b3da85L)]
    public interface MoneyDailyBonusTemplate : ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        MoneyBonusComponent moneyBonus();
    }
}

