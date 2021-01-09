namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class Tooltip : MonoBehaviour
    {
        public GameObject defaultTooltipContentPrefab;
        public float localPositionXOffset = 10f;
        public float maxWidth = 600f;
        public float marginX = 40f;
        public GameObject background;
        private GameObject tooltipContent;
        private RectTransform tooltipContentRect;
        private RectTransform thisRectTransform;

        private void Awake()
        {
            this.thisRectTransform = base.GetComponent<RectTransform>();
            base.gameObject.SetActive(false);
        }

        public void Hide()
        {
            if (this.tooltipContent != null)
            {
                CanvasGroup group = this.tooltipContent.GetComponent<CanvasGroup>();
                if (group != null)
                {
                    group.interactable = false;
                }
            }
            Animator component = base.GetComponent<Animator>();
            if (!base.gameObject.activeSelf)
            {
                Destroy(this.tooltipContent);
            }
            else
            {
                component.SetBool("show", false);
                Destroy(this.tooltipContent, component.GetCurrentAnimatorStateInfo(0).length);
            }
        }

        private void SetBackground(bool backgroundActive)
        {
            if (this.background != null)
            {
                this.background.SetActive(backgroundActive);
            }
            else
            {
                object[] args = new object[] { base.gameObject.name, base.transform.parent.gameObject.name };
                Debug.LogWarningFormat("Background reference wasn't set in tooltip {1}/{0}", args);
            }
        }

        public void Show(Vector3 canvasLocalPosition, object contentData, GameObject tooltipContentPrefab, bool backgroundActive)
        {
            this.SetBackground(backgroundActive);
            this.thisRectTransform.anchoredPosition = canvasLocalPosition;
            this.thisRectTransform.sizeDelta = new Vector2(this.maxWidth, this.thisRectTransform.sizeDelta.y);
            if (this.tooltipContent != null)
            {
                Destroy(this.tooltipContent);
            }
            this.tooltipContent = Instantiate<GameObject>((tooltipContentPrefab == null) ? this.defaultTooltipContentPrefab : tooltipContentPrefab);
            this.tooltipContent.transform.SetParent(base.transform, false);
            this.tooltipContent.GetComponent<ITooltipContent>().Init(contentData);
            this.tooltipContentRect = this.tooltipContent.GetComponent<RectTransform>();
            this.tooltipContent.SetActive(true);
            base.gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            if (this.tooltipContentRect.rect.width < (this.maxWidth - this.marginX))
            {
                this.thisRectTransform.sizeDelta = new Vector2(this.tooltipContentRect.rect.width, this.thisRectTransform.sizeDelta.y);
            }
            Vector2 referenceResolution = base.GetComponentInParent<CanvasScaler>().referenceResolution;
            Vector2 zero = Vector2.zero;
            zero.x = ((canvasLocalPosition.x + this.thisRectTransform.rect.width) <= (referenceResolution.x / 2f)) ? 0f : 1f;
            zero.y = ((canvasLocalPosition.y - this.thisRectTransform.rect.height) >= (-referenceResolution.y / 2f)) ? 1f : 0f;
            this.thisRectTransform.pivot = zero;
            if ((zero.x == 0f) && (zero.y == 1f))
            {
                canvasLocalPosition += new Vector3(this.localPositionXOffset, 0f);
            }
            this.thisRectTransform.anchoredPosition = canvasLocalPosition;
            base.GetComponent<Animator>().SetBool("show", true);
        }
    }
}

