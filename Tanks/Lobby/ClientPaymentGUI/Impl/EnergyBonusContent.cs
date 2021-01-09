namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.UI;

    public class EnergyBonusContent : DealItemContent
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private GameObject goBackText;
        [SerializeField]
        private CanvasGroup bottom;
        [SerializeField]
        private Sprite activeBonusSprite;
        [SerializeField]
        private Sprite inactiveBonusSprite;
        [SerializeField]
        private Image bannerImage;

        protected override void FillFromEntity(Entity entity)
        {
            if (entity.HasComponent<ImageItemComponent>())
            {
                string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                this.bannerImage.sprite = this.activeBonusSprite;
            }
            EnergyBonusComponent component = entity.GetComponent<EnergyBonusComponent>();
            int bonus = component.Bonus;
            if (this.Premium)
            {
                bonus = component.PremiumBonus;
            }
            base.title.text = string.Format(this.Title, bonus);
            base.price.text = this.Price;
            if (entity.HasComponent<TakenBonusComponent>())
            {
                this.SetBonusInactive();
            }
            base.FillFromEntity(entity);
        }

        public void SetBonusActive()
        {
            this.button.interactable = true;
            this.goBackText.SetActive(false);
            base.GetComponent<TextTimerComponent>().enabled = false;
            this.bottom.alpha = 1f;
            this.bannerImage.sprite = this.activeBonusSprite;
        }

        public void SetBonusInactive()
        {
            this.button.interactable = false;
            this.goBackText.SetActive(true);
            base.EndDate = base.Entity.GetComponent<ExpireDateComponent>().Date;
            TextTimerComponent component = base.GetComponent<TextTimerComponent>();
            component.EndDate = base.EndDate;
            component.ActiveWhenTimeIsUp = true;
            component.enabled = true;
            this.bottom.alpha = 0.2f;
            this.bannerImage.sprite = this.inactiveBonusSprite;
        }

        public virtual string Price { get; set; }

        public virtual string Title { get; set; }

        public bool Premium { get; set; }
    }
}

