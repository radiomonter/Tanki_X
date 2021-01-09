namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class MVPScreenUIComponent : BehaviourComponent
    {
        public static int ShowCounter;
        [SerializeField]
        private MVPUserMainInfoComponent userInfo;
        [SerializeField]
        private MVPMainStatComponent mainStat;
        [SerializeField]
        private MVPOtherStatComponent otherStat;
        [SerializeField]
        private MVPTankInfoComponent tankInfo;
        [SerializeField]
        private MVPModulesInfoComponent modulesInfo;
        [SerializeField]
        private TimerWithAction continueTimer;
        [SerializeField]
        private float timeIfMvpIsNotPlayer;
        [SerializeField]
        private float timeIfMvpIsPlayer;

        internal void SetModuleConfig(ModuleUpgradablePowerConfigComponent moduleConfig)
        {
            this.tankInfo.SetModuleConfig(moduleConfig);
        }

        internal void SetResults(UserResult mvp, BattleResultForClient battleResultForClient, bool mvpIsPlayer)
        {
            if (ShowCounter > 0)
            {
                this.continueTimer.gameObject.SetActive(false);
            }
            else
            {
                this.continueTimer.gameObject.SetActive(true);
                this.continueTimer.CurrentTime = !mvpIsPlayer ? this.timeIfMvpIsNotPlayer : this.timeIfMvpIsPlayer;
            }
            this.userInfo.Set(mvp);
            this.mainStat.Set(mvp, battleResultForClient);
            this.otherStat.Set(mvp, battleResultForClient);
            this.tankInfo.Set(mvp);
            this.modulesInfo.Set(mvp.Modules);
        }
    }
}

