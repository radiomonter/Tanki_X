namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientPayment.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(StarterPackTimerComponent))]
    public class StarterPackScreenUIComponent : PurchaseItemComponent
    {
        [SerializeField]
        private StarterPackElementComponent elementPrefab;
        [SerializeField]
        private RectTransform mainPreviewContainer;
        [SerializeField]
        private RectTransform previewContainer;
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI description;
        [SerializeField]
        private TextMeshProUGUI hurryUp;
        [SerializeField]
        private TextMeshProUGUI newPrice;
        [SerializeField]
        private TextMeshProUGUI mainItemDescription;
        private Entity packEntity;

        private GameObject AddItem(string title, long count, Transform parent, string spriteUid, bool needFrame = false, ItemRarityType rarity = 0)
        {
            GameObject obj2 = Instantiate<GameObject>(this.elementPrefab.gameObject);
            obj2.transform.SetParent(parent, false);
            StarterPackElementComponent component = obj2.GetComponent<StarterPackElementComponent>();
            component.title.text = title;
            component.previewSkin.gameObject.GetComponent<Image>().preserveAspect = true;
            component.previewSkin.SpriteUid = spriteUid;
            if (count <= 0L)
            {
                component.count.gameObject.SetActive(false);
            }
            else
            {
                component.count.gameObject.SetActive(true);
                component.count.text = "x" + count;
            }
            if (!needFrame)
            {
                component.RarityMask.enabled = false;
                component.RarityFrame.gameObject.SetActive(false);
                component.RarityMask.transform.localScale = new Vector3(1f, 1f, 1f);
                component.RarityFrame.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                component.RarityMask.enabled = true;
                component.RarityFrame.gameObject.SetActive(true);
                component.RarityFrame.SelectedSpriteIndex = (int) rarity;
                component.RarityMask.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                component.RarityFrame.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            return obj2;
        }

        private void AddMainItem(string title, long count, string spriteUid, bool needFrame = false, ItemRarityType rarity = 0)
        {
            this.mainPreviewContainer.gameObject.SetActive(true);
            this.AddItem(title, count, this.mainPreviewContainer, spriteUid, needFrame, rarity).transform.SetAsFirstSibling();
        }

        public void Clear()
        {
            base.methods.Clear();
        }

        private void ClearElements()
        {
            this.mainPreviewContainer.gameObject.SetActive(false);
            if (this.mainPreviewContainer.childCount > 1)
            {
                Destroy(this.mainPreviewContainer.GetChild(0).gameObject);
            }
            for (int i = 0; i < this.previewContainer.transform.childCount; i++)
            {
                if (this.previewContainer.GetChild(i).GetComponent<StarterPackElementComponent>() != null)
                {
                    Destroy(this.previewContainer.GetChild(i).gameObject);
                }
            }
        }

        public void Close()
        {
            MainScreenComponent.Instance.ShowMain();
        }

        public void OnClick()
        {
            base.OnPackClick(this.packEntity, false);
        }

        private void UpdateElements(Entity entity)
        {
            SpecialOfferContentLocalizationComponent component = entity.GetComponent<SpecialOfferContentLocalizationComponent>();
            this.title.text = component.Title;
            SpecialOfferScreenLocalizationComponent component2 = entity.GetComponent<SpecialOfferScreenLocalizationComponent>();
            this.hurryUp.text = component2.Footer;
            this.description.text = component2.Description;
            GoodsPriceComponent component3 = entity.GetComponent<GoodsPriceComponent>();
            this.newPrice.text = component3.Price + " " + component3.Currency;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) this.newPrice.rectTransform.parent.parent);
            ItemsPackFromConfigComponent component4 = entity.GetComponent<ItemsPackFromConfigComponent>();
            CountableItemsPackComponent component5 = entity.GetComponent<CountableItemsPackComponent>();
            XCrystalsPackComponent component6 = entity.GetComponent<XCrystalsPackComponent>();
            CrystalsPackComponent component7 = entity.GetComponent<CrystalsPackComponent>();
            List<long> itemIds = new List<long>(component4.Pack);
            itemIds.AddRange(component5.Pack.Keys);
            RequestInfoForItemsEvent evt = new RequestInfoForItemsEvent(itemIds);
            this.SendEvent<RequestInfoForItemsEvent>(evt, entity);
            this.AddMainItem(evt.mainItemTitle, (long) evt.mainItemCount, evt.mainItemSprite, false, ItemRarityType.COMMON);
            this.mainItemDescription.text = evt.mainItemDescription;
            foreach (long num in component5.Pack.Keys)
            {
                if (evt.mainItemId != num)
                {
                    string title = evt.titles[num];
                    string spriteUid = evt.previews[num];
                    this.AddItem(title, (long) component5.Pack[num], this.previewContainer.transform, spriteUid, evt.rarityFrames[num], evt.rarities[num]);
                }
            }
            foreach (long num3 in component4.Pack)
            {
                if (evt.mainItemId != num3)
                {
                    string title = evt.titles[num3];
                    string spriteUid = evt.previews[num3];
                    this.AddItem(title, 0L, this.previewContainer.transform, spriteUid, evt.rarityFrames[num3], evt.rarities[num3]);
                }
            }
            if ((component7.Total > 0L) && !evt.mainItemCrystal)
            {
                this.AddItem(evt.crystalTitle, component7.Total, this.previewContainer.transform, evt.crystalSprite, false, ItemRarityType.COMMON);
            }
            if (((component6.Amount + component6.Bonus) > 0L) && !evt.mainItemXCrystal)
            {
                this.AddItem(evt.xCrystalTitle, component6.Amount + component6.Bonus, this.previewContainer.transform, evt.xCrystalSprite, false, ItemRarityType.COMMON);
            }
        }

        private void UpdateTimer()
        {
            StarterPackTimerComponent component = base.GetComponent<StarterPackTimerComponent>();
            component.RunTimer((float) ((long) (this.packEntity.GetComponent<SpecialOfferEndTimeComponent>().EndDate - Date.Now)));
            component.onTimerExpired = new StarterPackTimerComponent.TimerExpired(this.Close);
        }

        public Entity PackEntity
        {
            get => 
                this.packEntity;
            set
            {
                this.packEntity = value;
                if (value != null)
                {
                    this.UpdateTimer();
                    this.ClearElements();
                    this.UpdateElements(this.packEntity);
                }
            }
        }
    }
}

