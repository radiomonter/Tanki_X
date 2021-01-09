namespace Tanks.Battle.ClientCore.API
{
    using System;
    using UnityEngine;

    public class BattleLabelBuilder
    {
        public static GameObject battleLabelPrefab;
        private GameObject battleLabelInstance;

        public BattleLabelBuilder(long battleId)
        {
            this.battleLabelInstance = this.InstantiateBattleLabel(battleId);
        }

        public GameObject Build()
        {
            this.battleLabelInstance.SetActive(true);
            return this.battleLabelInstance;
        }

        private GameObject InstantiateBattleLabel(long battleId)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(battleLabelPrefab);
            obj2.SetActive(false);
            obj2.GetComponent<BattleLabelComponent>().BattleId = battleId;
            return obj2;
        }
    }
}

