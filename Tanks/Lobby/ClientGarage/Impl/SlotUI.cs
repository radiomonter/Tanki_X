namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class SlotUI : MonoBehaviour
    {
        [SerializeField]
        private ImageSkin moduleIcon;
        [SerializeField]
        private PaletteColorField exceptionalColor;
        [SerializeField]
        private PaletteColorField epicColor;
        [SerializeField]
        private Image lockImage;
        private int unlockLevel;

        public void SetEpic()
        {
            this.moduleIcon.GetComponent<Image>().color = (Color) this.epicColor;
        }

        public void SetLegendary()
        {
            this.moduleIcon.GetComponent<Image>().color = (Color) this.exceptionalColor;
        }

        public void SetLockedTooltip(int unlockLevel)
        {
            TooltipShowBehaviour component = base.GetComponent<TooltipShowBehaviour>();
            if (component != null)
            {
                component.showTooltip = true;
                component.TipText = component.localizedTip.Value.Replace("{0}", unlockLevel.ToString());
            }
        }

        public void SetNormal()
        {
            this.moduleIcon.GetComponent<Image>().color = Color.white;
        }

        public void SetUnlockedTooltip(string name)
        {
            TooltipShowBehaviour component = base.GetComponent<TooltipShowBehaviour>();
            if (component != null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    component.showTooltip = false;
                }
                else
                {
                    component.showTooltip = true;
                    component.TipText = name;
                }
            }
        }

        public string Icon
        {
            set => 
                this.moduleIcon.SpriteUid = value;
        }

        public bool Locked
        {
            set
            {
                this.lockImage.gameObject.SetActive(value);
                this.moduleIcon.gameObject.SetActive(!value);
            }
        }
    }
}

