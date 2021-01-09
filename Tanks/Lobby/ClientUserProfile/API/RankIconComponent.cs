namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class RankIconComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Tanks.Lobby.ClientControls.API.ImageListSkin imageListSkin;

        public void SetRank(int rank)
        {
            this.imageListSkin.gameObject.SetActive(true);
            this.imageListSkin.SelectSprite(rank.ToString());
        }

        public Tanks.Lobby.ClientControls.API.ImageListSkin ImageListSkin =>
            this.imageListSkin;
    }
}

