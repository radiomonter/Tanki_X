namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientQuests.API;
    using TMPro;
    using UnityEngine;

    public class InBattleQuestItemGUIComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI taskText;
        [SerializeField]
        private ImageSkin taskImageSkin;
        [SerializeField]
        private TextMeshProUGUI currentProgressValue;
        [SerializeField]
        private TextMeshProUGUI targetProgressValue;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private InBattleQuestItemGUIRewardContainerComponent rewardContainer;
        private bool questCompleted;

        public void CompleteQuest()
        {
            this.animator.SetTrigger("Complete");
        }

        public void DestroyQuest()
        {
            Destroy(base.gameObject);
        }

        public void ProgressWasShown()
        {
            if (this.questCompleted)
            {
                this.CompleteQuest();
            }
        }

        public void SetImage(string spriteUid)
        {
            this.taskImageSkin.SpriteUid = spriteUid;
        }

        public void SetReward(BattleQuestReward reward, int quatity, long itemId)
        {
            this.rewardContainer.SetActiveReward(reward, quatity, itemId);
        }

        public void UpdateCurrentProgressValue(string newCurrentProgressValue, bool questCompleted = false)
        {
            this.questCompleted = questCompleted;
            this.CurrentProgressValue = newCurrentProgressValue;
            this.animator.SetTrigger("ShowProgress");
        }

        public string TaskText
        {
            get => 
                this.taskText.text;
            set => 
                this.taskText.text = value;
        }

        public string TargetProgressValue
        {
            get => 
                this.targetProgressValue.text;
            set => 
                this.targetProgressValue.text = value;
        }

        public string CurrentProgressValue
        {
            get => 
                this.currentProgressValue.text;
            set => 
                this.currentProgressValue.text = value;
        }
    }
}

