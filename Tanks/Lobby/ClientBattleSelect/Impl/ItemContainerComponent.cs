namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ItemContainerComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject itemContainer;
        [SerializeField]
        private GameObject itemPrefab;

        protected void ClearItems()
        {
            IEnumerator enumerator = this.itemContainer.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    Destroy(current.gameObject);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        protected void InstantiateItems(List<SpecialOfferItem> items)
        {
            foreach (SpecialOfferItem item in items)
            {
                SpecialOfferItemUiComponent component = Instantiate<GameObject>(this.itemPrefab, this.itemContainer.transform, false).GetComponent<SpecialOfferItemUiComponent>();
                component.title.text = item.Title;
                if (item.Quantity == 0)
                {
                    component.quantity.enabled = false;
                }
                else
                {
                    component.quantity.text = "x" + item.Quantity.ToString();
                }
                if (item.RibbonLabel == string.Empty)
                {
                    component.ribbon.gameObject.SetActive(false);
                }
                else
                {
                    component.ribbon.gameObject.SetActive(true);
                    component.ribbonLabel.text = item.RibbonLabel;
                }
                component.imageSkin.SpriteUid = item.SpriteUid;
            }
        }
    }
}

