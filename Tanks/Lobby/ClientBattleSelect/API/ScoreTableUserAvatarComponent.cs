namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class ScoreTableUserAvatarComponent : BehaviourComponent
    {
        [SerializeField]
        private bool enableShowUserProfileOnAvatarClick;

        public bool EnableShowUserProfileOnAvatarClick =>
            this.enableShowUserProfileOnAvatarClick;
    }
}

