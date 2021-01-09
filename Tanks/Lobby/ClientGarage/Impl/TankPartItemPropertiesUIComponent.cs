namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class TankPartItemPropertiesUIComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [SerializeField]
        private UpgradePropertyUI propertyUIPreafab;
        [SerializeField]
        private RectTransform contentRect;
        [SerializeField]
        private RectTransform arrowImageRect;
        [SerializeField]
        private float minContentWidth = 400f;
        [SerializeField]
        private float maxContentWidth = 500f;
        [SerializeField]
        private SelectedItemUI selectedItemUI;
        private bool isShow;
        private float arrowInitialScale;
        private bool hover;

        public void AddProperties(TankPartItem tankPartItem, float coef, float selectedCoef)
        {
            this.Clear();
            bool flag = true;
            foreach (VisualProperty property in tankPartItem.Properties)
            {
                float minValue = property.GetValue(0f) - (property.GetValue(0f) / 10f);
                float maxValue = property.GetValue(1f);
                float currentValue = property.GetValue(coef);
                string formatedValue = property.GetFormatedValue(coef);
                float nextValue = property.GetValue(selectedCoef);
                string nextValueStr = property.GetFormatedValue(selectedCoef);
                if (currentValue == nextValue)
                {
                    this.GetPropertyUi().SetValue(property.Name, property.Unit, formatedValue);
                    continue;
                }
                this.GetPropertyUi().SetUpgradableValue(property.Name, property.Unit, formatedValue, nextValueStr, currentValue, nextValue, minValue, maxValue, property.Format);
                flag = false;
            }
            if (!flag)
            {
                this.contentRect.sizeDelta = new Vector2(this.maxContentWidth, this.contentRect.sizeDelta.y);
            }
            else
            {
                this.contentRect.sizeDelta = new Vector2(this.minContentWidth, this.contentRect.sizeDelta.y);
                foreach (UpgradePropertyUI yui in this.contentRect.GetComponentsInChildren<UpgradePropertyUI>(true))
                {
                    yui.DisableNextValueAndArrow();
                    RectTransform component = yui.GetComponent<RectTransform>();
                    Vector2 sizeDelta = component.sizeDelta;
                    component.sizeDelta = new Vector2(this.minContentWidth - 20f, sizeDelta.y);
                }
            }
        }

        public void Clear()
        {
            UpgradePropertyUI[] componentsInChildren = this.contentRect.GetComponentsInChildren<UpgradePropertyUI>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].transform.SetParent(null);
                Destroy(componentsInChildren[i].gameObject);
            }
        }

        public UpgradePropertyUI GetPropertyUi()
        {
            UpgradePropertyUI yui = Instantiate<UpgradePropertyUI>(this.propertyUIPreafab);
            yui.gameObject.SetActive(true);
            yui.transform.SetParent(this.contentRect, false);
            return yui;
        }

        public void Hide()
        {
            this.contentRect.gameObject.SetActive(false);
            this.IsShow = false;
        }

        private void OnEnable()
        {
            this.arrowInitialScale = this.arrowImageRect.localScale.y;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.hover = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.hover = false;
        }

        public void Show(TankPartItem tankPartItem, float coef, float coefWithSelected)
        {
            if (this.isShow)
            {
                this.Hide();
            }
            else
            {
                tankPartItem ??= this.selectedItemUI.TankPartItem;
                this.AddProperties(tankPartItem, coef, coefWithSelected);
                this.contentRect.gameObject.SetActive(true);
                this.IsShow = true;
            }
        }

        private void Update()
        {
            if ((this.IsShow && (InputMapping.Cancel || (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))) && !this.hover)
            {
                this.Hide();
            }
        }

        private bool IsShow
        {
            get => 
                this.isShow;
            set
            {
                this.isShow = value;
                this.arrowImageRect.localScale = new Vector3(1f, !this.isShow ? this.arrowInitialScale : -this.arrowInitialScale, 1f);
            }
        }
    }
}

