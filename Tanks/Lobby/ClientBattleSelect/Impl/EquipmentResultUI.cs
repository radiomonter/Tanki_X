namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientProfile.API;
    using TMPro;
    using UnityEngine;

    public class EquipmentResultUI : ProgressResultUI
    {
        [SerializeField]
        private TextMeshProUGUI name;

        public void SetNewLevel()
        {
            base.SetResidualProgress();
        }

        public void SetProgress(Entity itemEntity, float expReward, int previousUpgradeLevel, int[] levels, BattleResultsTextTemplatesComponent textTemplates)
        {
            TankPartItem item = GarageItemsRegistry.GetItem<TankPartItem>(itemEntity.GetComponent<MarketItemGroupComponent>().Key);
            this.name.text = item.Name;
            bool flag2 = item.UpgradeLevel == UpgradablePropertiesUtils.MAX_LEVEL;
            LevelInfo currentLevelInfo = LevelInfo.Get((previousUpgradeLevel != UpgradablePropertiesUtils.MAX_LEVEL) ? (!flag2 ? ((long) item.AbsExperience) : ((long) levels[levels.Length - 1])) : (levels[levels.Length - 1] + ((long) expReward)), levels);
            base.SetProgress(expReward, levels, currentLevelInfo, textTemplates);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

