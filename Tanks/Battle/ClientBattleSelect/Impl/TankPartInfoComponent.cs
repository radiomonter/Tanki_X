namespace Tanks.Battle.ClientBattleSelect.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using TMPro;
    using UnityEngine;

    public class TankPartInfoComponent : MonoBehaviour
    {
        [SerializeField]
        private UpgradeStars stars;
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI icon;
        [SerializeField]
        private TextMeshProUGUI mainValue;
        [SerializeField]
        private TextMeshProUGUI additionalValue;
        [SerializeField]
        private TankPartModuleType tankPartType;

        private List<int[]> PrepareModules(List<ModuleInfo> modules)
        {
            <PrepareModules>c__AnonStorey0 storey = new <PrepareModules>c__AnonStorey0 {
                $this = this,
                res = new List<int[]>()
            };
            modules.ForEach(new Action<ModuleInfo>(storey.<>m__0));
            return storey.res;
        }

        public void Set(long id, List<ModuleInfo> modules, ModuleUpgradablePowerConfigComponent moduleConfig)
        {
            float power = TankUpgradeUtils.CalculateUpgradeCoeff(this.PrepareModules(modules), 3, moduleConfig);
            this.stars.SetPower(power);
            TankPartItem item = GarageItemsRegistry.GetItem<TankPartItem>(id);
            this.icon.text = "<sprite name=\"" + id + "\">";
            this.title.text = item.Name;
            VisualProperty property = item.Properties[0];
            this.mainValue.text = property.InitialValue.ToString();
            this.additionalValue.text = (power <= 0f) ? string.Empty : ("+ " + (property.GetValue(power) - property.InitialValue).ToString("0"));
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <PrepareModules>c__AnonStorey0
        {
            internal List<int[]> res;
            internal TankPartInfoComponent $this;

            internal void <>m__0(ModuleInfo m)
            {
                ModuleItem item = TankPartInfoComponent.GarageItemsRegistry.GetItem<ModuleItem>(m.ModuleId);
                if (item.TankPartModuleType == this.$this.tankPartType)
                {
                    int[] numArray = new int[] { item.TierNumber, (int) m.UpgradeLevel };
                    this.res.Add(numArray);
                }
            }
        }
    }
}

