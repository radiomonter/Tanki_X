namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class CellsProgressBar : MonoBehaviour
    {
        public GameObject emptyCell;
        public GameObject filledCell;
        public GameObject filledEpicCell;

        private void CreateFromPrefab(GameObject prefab)
        {
            Instantiate<GameObject>(prefab).transform.SetParent(base.transform, false);
        }

        private DailyBonusData getBonusData(long receivedReward, DailyBonusData[] bonusData)
        {
            <getBonusData>c__AnonStorey0 storey = new <getBonusData>c__AnonStorey0 {
                receivedReward = receivedReward
            };
            return bonusData.First<DailyBonusData>(new Func<DailyBonusData, bool>(storey.<>m__0));
        }

        public void Init(int capacity, DailyBonusData[] bonusData, List<long> receivedRewards)
        {
            base.transform.DestroyChildren();
            foreach (long num in receivedRewards)
            {
                DailyBonusData data = this.getBonusData(num, bonusData);
                this.CreateFromPrefab(!data.IsEpic() ? this.filledCell : this.filledEpicCell);
            }
            int num2 = capacity - receivedRewards.Count;
            for (int i = 0; i < num2; i++)
            {
                this.CreateFromPrefab(this.emptyCell);
            }
        }

        private void OnDisable()
        {
            base.transform.DestroyChildren();
        }

        [CompilerGenerated]
        private sealed class <getBonusData>c__AnonStorey0
        {
            internal long receivedReward;

            internal bool <>m__0(DailyBonusData it) => 
                it.Code == this.receivedReward;
        }
    }
}

