namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1515ccaca97L)]
    public interface PerformanceStatisticsTemplate : Template
    {
        [AutoAdded]
        PerformanceStatisticsHelperComponent perfomanceStatisticsHelper();
        [AutoAdded, PersistentConfig("", false)]
        PerformanceStatisticsSettingsComponent performanceStatisticsSettings();
    }
}

