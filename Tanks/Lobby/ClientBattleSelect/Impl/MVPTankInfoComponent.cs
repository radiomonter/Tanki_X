namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class MVPTankInfoComponent : MonoBehaviour
    {
        [SerializeField]
        private TankPartInfoComponent hull;
        [SerializeField]
        private TankPartInfoComponent turret;
        private ModuleUpgradablePowerConfigComponent moduleConfig;

        public void Set(UserResult mvp)
        {
            this.hull.Set(mvp.HullId, mvp.Modules, this.moduleConfig);
            this.turret.Set(mvp.WeaponId, mvp.Modules, this.moduleConfig);
        }

        internal void SetModuleConfig(ModuleUpgradablePowerConfigComponent moduleConfig)
        {
            this.moduleConfig = moduleConfig;
        }
    }
}

