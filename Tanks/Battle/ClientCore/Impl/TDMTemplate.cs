namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x7204dd5d896934faL)]
    public interface TDMTemplate : TeamBattleTemplate, BattleTemplate, Template
    {
        TDMComponent tdmComponent();
    }
}

