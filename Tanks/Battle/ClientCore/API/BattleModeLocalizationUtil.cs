namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class BattleModeLocalizationUtil
    {
        private static Dictionary<BattleMode, string> modeToName;

        private static void CheckAndCreate()
        {
            modeToName ??= ConfiguratorService.GetConfig("localization/battle_mode").ConvertTo<GameModesDescriptionData>().battleModeLocalization;
        }

        public static string GetLocalization(BattleMode mode)
        {
            CheckAndCreate();
            return modeToName[mode];
        }

        public static Dictionary<BattleMode, string> GetModeToNameDict()
        {
            CheckAndCreate();
            return modeToName;
        }

        [Inject]
        public static ConfigurationService ConfiguratorService { get; set; }
    }
}

