namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x16073a03d82L)]
    public interface QuestDailyBonusTemplate : Template
    {
        [AutoAdded]
        QuestExchangeBonusComponent questExchangeBonus();
    }
}

