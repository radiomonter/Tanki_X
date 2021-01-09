namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class CardPriceLabelComponent : UIBehaviour, Component
    {
        [SerializeField]
        private Text[] resourceCountTexts;
        [SerializeField]
        private GameObject[] spacingObjects;
        private int[] prices = new int[1];
        private long[] counts = new long[1];
        [SerializeField]
        private Color textColorWhenResourceEnough = Color.green;
        [SerializeField]
        private Color textColorWhenResourceNotEnough = Color.red;
        private bool enoughCards;

        private Color GetColor(int price, long count) => 
            (count < price) ? this.textColorWhenResourceNotEnough : this.textColorWhenResourceEnough;

        private string GetText(int price, long count) => 
            count + " / " + price;

        private void SetPrice(long type, long price)
        {
            int index = 0;
            this.prices[index] = (int) price;
            this.enoughCards = this.enoughCards && (this.prices[index] <= this.counts[index]);
            this.resourceCountTexts[index].text = this.GetText(this.prices[index], this.counts[index]);
            this.resourceCountTexts[index].color = this.GetColor(this.prices[index], this.counts[index]);
            this.resourceCountTexts[index].gameObject.SetActive(true);
            this.spacingObjects[index].SetActive(true);
        }

        public void SetPrices(ModuleCardsCompositionComponent moduleResourcesComponent)
        {
            this.enoughCards = true;
            for (int i = 0; i < this.prices.Length; i++)
            {
                this.resourceCountTexts[i].gameObject.SetActive(false);
                this.spacingObjects[i].SetActive(false);
                this.prices[i] = 0;
            }
            this.SetPrice(0x1e0f3L, (long) moduleResourcesComponent.CraftPrice.Cards);
        }

        public void SetRefund(long type, long count)
        {
            int index = (byte) type;
            this.counts[index] = count;
            this.resourceCountTexts[index].text = count.ToString();
            this.resourceCountTexts[index].color = this.textColorWhenResourceEnough;
            this.resourceCountTexts[index].gameObject.SetActive(true);
            this.spacingObjects[index].SetActive(true);
        }

        public void SetUserCardsCount(long count)
        {
            int index = 0;
            this.counts[index] = count;
            this.resourceCountTexts[index].text = this.GetText(this.prices[index], this.counts[index]);
            this.resourceCountTexts[index].color = this.GetColor(this.prices[index], this.counts[index]);
        }

        public bool EnoughCards =>
            this.enoughCards;
    }
}

