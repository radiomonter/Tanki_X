namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x8d3386b286cda46L)]
    public interface BattleScreenTemplate : Template
    {
        BattleScreenComponent battleScreen();
    }
}

