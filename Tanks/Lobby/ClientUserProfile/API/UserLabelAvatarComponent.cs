namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class UserLabelAvatarComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private ImageSkin avatarImage;
        [SerializeField]
        private Color offlineColor;
        [SerializeField]
        private Color onlineColor;
        [SerializeField]
        private ImageListSkin _avatarFrame;

        public Color OfflineColor =>
            this.offlineColor;

        public Color OnlineColor =>
            this.onlineColor;

        public ImageSkin AvatarImage =>
            this.avatarImage;

        public bool IsPremium
        {
            set
            {
                if (this._avatarFrame)
                {
                    this._avatarFrame.SelectedSpriteIndex = !value ? 0 : 3;
                }
            }
        }

        public bool IsSelf
        {
            set
            {
                if (this._avatarFrame && value)
                {
                    this._avatarFrame.SelectedSpriteIndex = 1;
                }
            }
        }
    }
}

