namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class BattleResultRewardCheckComponent : BehaviourComponent
    {
        [SerializeField]
        private long quickBattleEndTutorialId;

        public long QuickBattleEndTutorialId =>
            this.quickBattleEndTutorialId;
    }
}

