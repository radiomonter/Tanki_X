namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Battle.ClientCore.API;
    using TMPro;
    using UnityEngine;

    public class MVPMainStatComponent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI killsCount;
        [SerializeField]
        private TextMeshProUGUI assistsCount;
        [SerializeField]
        private TextMeshProUGUI deathsCount;
        [SerializeField]
        private GameObject kills;
        [SerializeField]
        private GameObject assists;
        [SerializeField]
        private GameObject deaths;

        public void Set(UserResult mvp, BattleResultForClient battleResultForClient)
        {
            this.assists.SetActive(battleResultForClient.BattleMode != BattleMode.DM);
            this.killsCount.SetText(mvp.Kills.ToString());
            this.assistsCount.SetText(mvp.KillAssists.ToString());
            this.deathsCount.SetText(mvp.Deaths.ToString());
        }
    }
}

