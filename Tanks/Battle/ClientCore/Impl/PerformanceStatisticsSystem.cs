namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;
    using UnityEngine.Profiling;

    public class PerformanceStatisticsSystem : ECSSystem
    {
        public const string CONFIG_PATH = "service/performancestatistics";

        [OnEventFire]
        public void CreatePerformanceStatisticsEntity(NodeAddedEvent e, SingleNode<SelfUserComponent> selfUser)
        {
            base.CreateEntity<PerformanceStatisticsTemplate>("service/performancestatistics");
        }

        private static string GetMapName(Node map)
        {
            string configPath = ((EntityInternal) map.Entity).TemplateAccessor.Get().ConfigPath;
            int num = configPath.LastIndexOf("/", StringComparison.Ordinal);
            return ((num <= 0) ? configPath : configPath.Substring(num + 1));
        }

        [OnEventFire]
        public void InitMeasuringOnRoundStart(NodeAddedEvent e, RoundUserNode roundUser, [Context, JoinByUser] BattleUserNode selfBattleUser, [JoinAll] StatisticsNode statistics)
        {
            PerformanceStatisticsHelperComponent performanceStatisticsHelper = statistics.performanceStatisticsHelper;
            PerformanceStatisticsSettingsComponent performanceStatisticsSettings = statistics.performanceStatisticsSettings;
            performanceStatisticsHelper.startRoundTimeInSec = UnityTime.realtimeSinceStartup;
            performanceStatisticsHelper.frames = new FramesCollection(performanceStatisticsSettings.HugeFrameDurationInMs, performanceStatisticsSettings.MeasuringIntervalInSec);
            performanceStatisticsHelper.tankCount = new StatisticCollection(50);
        }

        private static bool RoundTimeTooShortForMeasuring(StatisticsNode statistics)
        {
            float startRoundTimeInSec = statistics.performanceStatisticsHelper.startRoundTimeInSec;
            int delayInSecBeforeMeasuringStarted = statistics.performanceStatisticsSettings.DelayInSecBeforeMeasuringStarted;
            return ((UnityTime.realtimeSinceStartup - startRoundTimeInSec) < delayInSecBeforeMeasuringStarted);
        }

        [OnEventFire]
        public void SendStatisticDataOnRoundStop(NodeRemoveEvent e, SingleNode<RoundUserComponent> roundUser, [JoinByUser] BattleUserNode battleUser, [JoinByUser] SelfUserNode selfUser, [JoinByUser] SingleNode<RoundUserComponent> node, [JoinByBattle] SingleNode<BattleComponent> battle, [JoinByMap] SingleNode<MapComponent> map, [JoinAll] StatisticsNode statistics)
        {
            if (!RoundTimeTooShortForMeasuring(statistics))
            {
                PerformanceStatisticsHelperComponent performanceStatisticsHelper = statistics.performanceStatisticsHelper;
                FramesCollection frames = performanceStatisticsHelper.frames;
                PerformanceStatisticData data = new PerformanceStatisticData {
                    UserName = selfUser.userUid.Uid,
                    GraphicDeviceName = SystemInfo.graphicsDeviceName,
                    GraphicsDeviceType = SystemInfo.graphicsDeviceType.ToString(),
                    GraphicsMemorySize = SystemInfo.graphicsMemorySize,
                    DefaultQuality = GraphicsSettings.INSTANCE.DefaultQuality.Name,
                    Quality = QualitySettings.names[QualitySettings.GetQualityLevel()],
                    Resolution = GraphicsSettings.INSTANCE.CurrentResolution.ToString(),
                    MapName = GetMapName(map),
                    BattleRoundTimeInMin = (int) ((Time.realtimeSinceStartup - performanceStatisticsHelper.startRoundTimeInSec) / 60f),
                    TankCountModa = performanceStatisticsHelper.tankCount.Moda,
                    Moda = frames.Moda,
                    Average = frames.Average,
                    StandardDeviationInMs = frames.StandartDevation,
                    HugeFrameCount = frames.HugeFrameCount,
                    MinAverageForInterval = frames.MinAverageForInterval,
                    MaxAverageForInterval = frames.MaxAverageForInterval,
                    GraphicDeviceKey = $"DeviceVendorID: {SystemInfo.graphicsDeviceVendorID}; DeviceID: {SystemInfo.graphicsDeviceID}",
                    AveragePing = battleUser.battlePing.getAveragePing(),
                    PingModa = battleUser.battlePing.getMediana(),
                    GraphicsDeviceVersion = SystemInfo.graphicsDeviceVersion,
                    CustomSettings = GraphicsSettings.INSTANCE.customSettings,
                    Windowed = !Screen.fullScreen,
                    SaturationLevel = GraphicsSettings.INSTANCE.CurrentSaturationLevel,
                    VegetationLevel = GraphicsSettings.INSTANCE.CurrentVegetationLevel,
                    GrassLevel = GraphicsSettings.INSTANCE.CurrentGrassLevel,
                    AntialiasingQuality = GraphicsSettings.INSTANCE.CurrentAntialiasingQuality,
                    AnisotropicQuality = GraphicsSettings.INSTANCE.CurrentAnisotropicQuality,
                    TextureQuality = GraphicsSettings.INSTANCE.CurrentTextureQuality,
                    ShadowQuality = GraphicsSettings.INSTANCE.CurrentShadowQuality,
                    AmbientOcclusion = GraphicsSettings.INSTANCE.currentAmbientOcclusion,
                    Bloom = GraphicsSettings.INSTANCE.currentBloom,
                    RenderResolutionQuality = GraphicsSettings.INSTANCE.CurrentRenderResolutionQuality,
                    SystemMemorySize = SystemInfo.systemMemorySize,
                    TotalReservedMemory = (long) Profiler.GetTotalReservedMemory(),
                    TotalAllocatedMemory = (long) Profiler.GetTotalAllocatedMemory(),
                    MonoHeapSize = (long) Profiler.GetMonoHeapSize(),
                    HandlerNames = new string[0],
                    HandlerCallCounts = new int[0]
                };
                base.Log.InfoFormat("{0}\n{1}", "PerformanceStatisticData", EcsToStringUtil.ToStringWithProperties(data, ", "));
                base.ScheduleEvent(new SendPerfomanceStatisticDataEvent(data), selfUser);
            }
        }

        [OnEventFire]
        public void Update(TimeUpdateEvent e, BattleUserNode selfBattleUser, [JoinByUser] SingleNode<RoundUserComponent> selfRoundUser, [JoinByBattle] ICollection<SingleNode<RoundUserComponent>> allRoundUsers, [JoinAll] StatisticsNode statistics)
        {
            if (!RoundTimeTooShortForMeasuring(statistics))
            {
                PerformanceStatisticsHelperComponent performanceStatisticsHelper = statistics.performanceStatisticsHelper;
                int durationInMs = (int) (e.DeltaTime * 1000f);
                performanceStatisticsHelper.frames.AddFrame(durationInMs);
                performanceStatisticsHelper.tankCount.Add(allRoundUsers.Count, durationInMs);
            }
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class BattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
            public BattlePingComponent battlePing;
        }

        public class RoundUserNode : Node
        {
            public RoundUserComponent roundUser;
            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserUidComponent userUid;
        }

        public class StatisticsNode : Node
        {
            public PerformanceStatisticsSettingsComponent performanceStatisticsSettings;
            public PerformanceStatisticsHelperComponent performanceStatisticsHelper;
        }
    }
}

