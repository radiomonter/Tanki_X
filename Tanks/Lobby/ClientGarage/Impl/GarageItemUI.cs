namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class GarageItemUI : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
    {
        private Carousel carousel;
        [SerializeField]
        private ImageSkin preview;
        [SerializeField]
        private ImageSkin shadow;
        private bool state;

        public void Deselect()
        {
            this.state = false;
            if (base.gameObject.activeInHierarchy)
            {
                base.GetComponent<Animator>().SetBool("Selected", this.state);
            }
        }

        public void Init(GarageItem item, Carousel carousel)
        {
            this.Item = item;
            this.preview.SpriteUid = item.Preview;
            this.shadow.SpriteUid = item.Preview;
            this.state = false;
            this.carousel = carousel;
        }

        private void OnEnable()
        {
            base.GetComponent<Animator>().SetBool("Selected", this.state);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!TutorialCanvas.Instance.IsShow)
            {
                this.carousel.Select(this.Item, false);
            }
        }

        public void Select()
        {
            this.state = true;
            base.GetComponent<Animator>().SetBool("Selected", this.state);
            this.SendEvent<ListItemSelectedEvent>((this.Item.UserItem == null) ? this.Item.MarketItem : this.Item.UserItem);
        }

        public UnityEngine.RectTransform RectTransform =>
            base.GetComponent<UnityEngine.RectTransform>();

        public GarageItem Item { get; private set; }
    }
}

