namespace Tanks.Lobby.ClientQuests.Impl
{
    using System;
    using Tanks.Lobby.ClientQuests.API;
    using UnityEngine;

    public class InBattleQuestItemGUIRewardContainerComponent : MonoBehaviour
    {
        [SerializeField]
        private InBattleQuestItemGUIRewardComponent itemReward;
        [SerializeField]
        private InBattleQuestItemGUIRewardComponent experienceReward;
        [SerializeField]
        private InBattleQuestItemGUIRewardComponent crystalReward;

        public void SetActiveReward(BattleQuestReward reward, int quantity, long itemId)
        {
            this.itemReward.gameObject.SetActive(false);
            this.experienceReward.gameObject.SetActive(false);
            this.crystalReward.gameObject.SetActive(false);
            switch (reward)
            {
                case BattleQuestReward.CRYSTALS:
                    this.crystalReward.gameObject.SetActive(true);
                    this.crystalReward.SetReward(quantity);
                    break;

                case BattleQuestReward.TURRET_EXP:
                case BattleQuestReward.HULL_EXP:
                    this.itemReward.gameObject.SetActive(true);
                    this.itemReward.SetReward(quantity, itemId);
                    break;

                case BattleQuestReward.RANK_EXP:
                    this.experienceReward.gameObject.SetActive(true);
                    this.experienceReward.SetReward(quantity);
                    break;

                case BattleQuestReward.CHEST_SCORE:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("reward");
            }
        }
    }
}

