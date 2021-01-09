namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class TankUpgradeUtils
    {
        private static int CalculateMaximumPercentSum(ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig, int slotCount)
        {
            int num = moduleUpgradablePowerConfig.Level2PowerByTier.Count - 1;
            List<int> list = moduleUpgradablePowerConfig.Level2PowerByTier[num];
            return (list[list.Count - 1] * slotCount);
        }

        public static float CalculateUpgradeCoeff(List<int[]> modulesParams, int slotCount, ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig)
        {
            int num = CalculateMaximumPercentSum(moduleUpgradablePowerConfig, slotCount);
            return ((num >= 0) ? (((float) CollectPercentSum(modulesParams, moduleUpgradablePowerConfig)) / ((float) num)) : -1f);
        }

        private static int CollectPercentSum(List<int[]> modulesParams, ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig)
        {
            int num = 0;
            List<List<int>> list = moduleUpgradablePowerConfig.Level2PowerByTier;
            foreach (int[] numArray in modulesParams)
            {
                int num2 = numArray[0];
                int num3 = Mathf.Min(numArray[1], list[num2].Count - 1);
                num += list[num2][num3];
            }
            return num;
        }
    }
}

