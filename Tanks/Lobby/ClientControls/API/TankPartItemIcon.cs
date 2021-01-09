namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TankPartItemIcon : BaseCombatLogMessageElement
    {
        [SerializeField]
        private TMP_SpriteAsset icons;
        [SerializeField]
        private Image image;

        public void SetIconWithName(string name)
        {
            foreach (TMP_Sprite sprite in this.icons.spriteInfoList)
            {
                if (sprite.name.Equals(name))
                {
                    this.image.sprite = sprite.sprite;
                }
            }
        }
    }
}

