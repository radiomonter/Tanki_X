namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class UserRankRestrictionBadgeGUIComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private ImageListSkin imageListSkin;

        public void SetRank(int rank)
        {
            this.imageListSkin.SelectSprite(rank.ToString());
        }
    }
}

