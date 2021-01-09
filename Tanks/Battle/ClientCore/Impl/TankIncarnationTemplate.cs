namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1582519e433L)]
    public interface TankIncarnationTemplate : Template
    {
        TankIncarnationKillStatisticsComponent tankIncarnationKillStatistics();
    }
}

