namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using UnityEngine;

    public class ProfileStatsSectionUIComponent : BehaviourComponent
    {
        [SerializeField]
        private RankUI rank;
        [SerializeField]
        private LeagueUIComponent league;

        public void SetRank(LevelInfo levelInfo)
        {
        }
    }
}

