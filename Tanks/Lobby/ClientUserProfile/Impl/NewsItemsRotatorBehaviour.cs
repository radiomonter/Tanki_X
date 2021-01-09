namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class NewsItemsRotatorBehaviour : UIBehaviour
    {
        public float swapPeriod = 10f;
        public float swapTime = 0.5f;
        public bool swapTrigger;
        public RectMask2D mask;
        private float swapBeginTime;
        private bool swapping;

        private static bool IsManualSwap() => 
            Input.GetMouseButtonDown(1);

        protected override void OnEnable()
        {
            this.swapBeginTime = Time.time;
        }

        private void OverlapFix(Transform child)
        {
            if (this.swapping && child.gameObject.activeSelf)
            {
                child.GetComponent<Animator>().SetTrigger("Normal");
            }
        }

        private void SetOffset(Transform child, float itemSize, float offset)
        {
            RectTransform transform = (RectTransform) child;
            float b = itemSize * offset;
            Vector2 anchoredPosition = transform.anchoredPosition;
            if (!Mathf.Approximately(anchoredPosition.x, b))
            {
                anchoredPosition.x = b;
                transform.anchoredPosition = anchoredPosition;
            }
        }

        public void SwapItems()
        {
            if (((base.transform.childCount >= 2) && !this.swapping) && (!((RectTransform) base.transform.GetChild(0)).GetComponent<NewsItemUIComponent>().noSwap || IsManualSwap()))
            {
                this.swapBeginTime = Time.time;
                this.swapping = true;
                this.mask.enabled = true;
            }
        }

        private void Update()
        {
            if (this.swapTrigger || ((Time.time >= (this.swapBeginTime + this.swapPeriod)) || IsManualSwap()))
            {
                this.SwapItems();
                this.swapTrigger = false;
            }
            float num = 0f;
            float itemSize = 0f;
            if (base.transform.childCount > 1)
            {
                RectTransform child = (RectTransform) base.transform.GetChild(0);
                itemSize = child.rect.width;
                if (this.swapping)
                {
                    if (Time.time < (this.swapBeginTime + this.swapTime))
                    {
                        num = (Time.time - this.swapBeginTime) / this.swapTime;
                    }
                    else
                    {
                        this.swapping = false;
                        child.SetAsLastSibling();
                        child.gameObject.SetActive(false);
                        this.mask.enabled = false;
                    }
                }
            }
            for (int i = 0; i < base.transform.childCount; i++)
            {
                float offset = i - num;
                Transform child = base.transform.GetChild(i);
                bool flag = (i == 0) || ((i == 1) && this.swapping);
                if (flag != child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(flag);
                    if (flag)
                    {
                        NewsItemComponent component = child.GetComponent<EntityBehaviour>().Entity.GetComponent<NewsItemComponent>();
                        component.ShowCount++;
                    }
                }
                this.SetOffset(child, itemSize, offset);
                this.OverlapFix(child);
            }
        }
    }
}

