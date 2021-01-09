namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class QuestRewardGUIComponent : BehaviourComponent
    {
        [SerializeField]
        private ImageSkin rewardImageSkin;
        [SerializeField]
        private TextMeshProUGUI rewardInfoText;
        [SerializeField]
        private CanvasGroup rewardCanvasGroup;
        [SerializeField]
        private float rewardedAlpha;

        public void SetImage(string spriteUid)
        {
            this.rewardImageSkin.SpriteUid = spriteUid;
            this.rewardImageSkin.enabled = true;
        }

        public void SetNotRewarded()
        {
            this.rewardCanvasGroup.alpha = 1f;
        }

        public void SetRewarded()
        {
            this.rewardCanvasGroup.alpha = this.rewardedAlpha;
        }

        public string RewardInfoText
        {
            set => 
                this.rewardInfoText.text = value;
        }
    }
}

