namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientProfile.API;
    using UnityEngine;

    public class SelfUserAvatarComponent : BehaviourComponent
    {
        [SerializeField]
        private ImageSkin avatar;
        [SerializeField]
        private ImageListSkin border;
        [SerializeField]
        private ImageListSkin rank;

        public void SetAvatar(string spriteUid)
        {
            if (this.avatar == null)
            {
                string[] textArray1 = new string[] { this.rank.gameObject.name, ":", this.rank.transform.parent.name, ":", this.rank.transform.parent.parent.name, ":", this.rank.transform.parent.parent.parent.name };
                Debug.LogError(string.Concat(textArray1));
            }
            this.avatar.SpriteUid = spriteUid;
        }

        public void SetLeagueBorder(int league)
        {
            this.border.SelectSprite(league.ToString());
        }

        public void SetRank()
        {
            LevelInfo info = this.SendEvent<GetUserLevelInfoEvent>(SelfUserComponent.SelfUser).Info;
            this.SetRank(info.Level + 1);
        }

        public void SetRank(int level)
        {
            this.rank.SelectSprite(level.ToString());
        }
    }
}

