namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using TMPro;
    using UnityEngine;

    public class MVPUserMainInfoComponent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI nickname;
        [SerializeField]
        private ImageSkin avatar;
        [SerializeField]
        private ImageListSkin league;
        [SerializeField]
        private RankIconComponent rank;

        public void Set(UserResult mvp)
        {
            this.nickname.SetText(mvp.Uid.Replace("botxz_", string.Empty));
            this.league.SelectedSpriteIndex = (mvp.League == null) ? 0 : mvp.League.GetComponent<LeagueConfigComponent>().LeagueIndex;
            this.rank.SetRank(mvp.Rank);
            this.avatar.SpriteUid = mvp.AvatarId;
        }
    }
}

