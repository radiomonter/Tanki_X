namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ModuleVisualProperty
    {
        public ModuleVisualProperty()
        {
            this.Name = string.Empty;
            this.UpgradeLevel2Values = new float[2];
            this.Unit = string.Empty;
            this.Format = string.Empty;
        }

        public float CalculateModuleEffectPropertyValue(int moduleUpgradeLevel, int maxModuleUpgradeLevel)
        {
            int length = this.UpgradeLevel2Values.Length;
            if (moduleUpgradeLevel > maxModuleUpgradeLevel)
            {
                moduleUpgradeLevel = maxModuleUpgradeLevel;
            }
            if (!this.LinearInterpolation)
            {
                return this.UpgradeLevel2Values[moduleUpgradeLevel];
            }
            float num2 = ((float) moduleUpgradeLevel) / ((float) maxModuleUpgradeLevel);
            float num3 = this.UpgradeLevel2Values[0];
            return (num3 + ((this.UpgradeLevel2Values[length - 1] - num3) * num2));
        }

        public string Name { get; set; }

        public bool LinearInterpolation { get; set; }

        public float[] UpgradeLevel2Values { get; set; }

        public Dictionary<long, EffectProperty> EquipmentProperties { get; set; }

        public string Unit { get; set; }

        [ProtocolOptional]
        public string Format { get; set; }

        public bool MaxAndMin { get; set; }

        public string[] MaxAndMinString { get; set; }

        public bool ProgressBar { get; set; }

        public string SpriteUID { get; set; }

        public bool Upgradable =>
            (this.UpgradeLevel2Values.Length > 0) && (this.UpgradeLevel2Values[0] != this.UpgradeLevel2Values[this.UpgradeLevel2Values.Length - 1]);
    }
}

