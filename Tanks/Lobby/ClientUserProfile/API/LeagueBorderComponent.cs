namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class LeagueBorderComponent : BehaviourComponent
    {
        [SerializeField]
        private Tanks.Lobby.ClientControls.API.ImageListSkin imageListSkin;

        public void SetLeague(int league)
        {
            this.imageListSkin.gameObject.SetActive(true);
            this.imageListSkin.SelectSprite(league.ToString());
        }

        public Tanks.Lobby.ClientControls.API.ImageListSkin ImageListSkin =>
            this.imageListSkin;
    }
}

