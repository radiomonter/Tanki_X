namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class LoginRewardItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [SerializeField]
        private TextMeshProUGUI dayLabel;
        [SerializeField]
        private LoginRewardItemTooltip tooltip;
        [SerializeField]
        private ImageSkin imagePrefab;
        [SerializeField]
        private Transform imagesContainer;
        [SerializeField]
        private LoginRewardProgressBar progressBar;
        [SerializeField]
        private LocalizedField dayLocalizedField;

        public void AddItem(string imageUID, string name)
        {
            ImageSkin skin = Instantiate<ImageSkin>(this.imagePrefab, this.imagesContainer);
            skin.SpriteUid = imageUID;
            skin.gameObject.SetActive(true);
            string str = (!string.IsNullOrEmpty(this.tooltip.Text) ? "\n" : string.Empty) + name;
            this.tooltip.Text = this.tooltip.Text + str;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            base.GetComponent<Animator>().SetBool("hover", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            base.GetComponent<Animator>().SetBool("hover", false);
        }

        public void SetupLines(bool itemIsFirst, bool itemIsLast)
        {
            if (itemIsFirst)
            {
                this.progressBar.LeftLine.SetActive(false);
            }
            if (itemIsLast)
            {
                this.progressBar.RightLine.SetActive(false);
            }
        }

        public int Day
        {
            set => 
                this.dayLabel.text = value + " " + this.dayLocalizedField.Value.ToUpper();
        }

        public LoginRewardProgressBar.FillType fillType
        {
            set => 
                this.progressBar.Fill(value);
        }
    }
}

