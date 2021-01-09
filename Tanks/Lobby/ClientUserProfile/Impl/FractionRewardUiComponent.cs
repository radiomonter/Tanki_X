namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class FractionRewardUiComponent : BehaviourComponent
    {
        [SerializeField]
        private ImageSkin _rewardImage;

        public string RewardImageUid
        {
            set => 
                this._rewardImage.SpriteUid = value;
        }
    }
}

