namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class ColoredProgressBar : MonoBehaviour
    {
        [Range(0f, 1f), SerializeField]
        private float initialProgress;
        [Range(0f, 1f), SerializeField]
        private float coloredProgress;
        [SerializeField]
        private RectTransform initialMask;
        [SerializeField]
        private RectTransform initialFiller;
        [SerializeField]
        private RectTransform coloredMask;
        [SerializeField]
        private RectTransform coloredInnerMask;
        [SerializeField]
        private RectTransform coloredFiller;
        private Image initialMaskImage;
        private Image coloredMaskImage;
        private Image coloredInnerMaskImage;

        private void Awake()
        {
            this.initialMaskImage = this.initialMask.GetComponent<Image>();
            this.coloredMaskImage = this.coloredMask.GetComponent<Image>();
            this.coloredInnerMaskImage = this.coloredInnerMask.GetComponent<Image>();
        }

        private void CreateIfAbsent(ref RectTransform child, Transform parent, string name)
        {
            if (child == null)
            {
                child = parent.Find(name) as RectTransform;
                if (child == null)
                {
                    child = new GameObject(name).AddComponent<RectTransform>();
                    child.SetParent(parent, false);
                }
            }
        }

        private void Reset()
        {
            this.CreateIfAbsent(ref this.initialMask, base.transform, "InitialMask");
            if (this.initialMask.GetComponent<Mask>() == null)
            {
                this.initialMask.gameObject.AddComponent<Mask>().showMaskGraphic = false;
            }
            Image component = this.initialMask.GetComponent<Image>();
            if (component == null)
            {
                component = this.initialMask.gameObject.AddComponent<Image>();
            }
            component.fillMethod = Image.FillMethod.Horizontal;
            component.fillOrigin = 0;
            component.type = Image.Type.Filled;
            this.Stretch(this.initialMask);
            this.CreateIfAbsent(ref this.initialFiller, this.initialMask, "Filler");
            if (this.initialFiller.GetComponent<Image>() == null)
            {
                this.initialFiller.gameObject.AddComponent<Image>();
            }
            this.Stretch(this.initialFiller);
            this.CreateIfAbsent(ref this.coloredMask, base.transform, "ColoredMask");
            if (this.coloredMask.GetComponent<Mask>() == null)
            {
                this.coloredMask.gameObject.AddComponent<Mask>().showMaskGraphic = false;
            }
            component = this.coloredMask.GetComponent<Image>();
            if (component == null)
            {
                component = this.coloredMask.gameObject.AddComponent<Image>();
            }
            component.fillMethod = Image.FillMethod.Horizontal;
            component.fillOrigin = 0;
            component.type = Image.Type.Filled;
            this.Stretch(this.coloredMask);
            this.CreateIfAbsent(ref this.coloredInnerMask, this.coloredMask, "InnerMask");
            if (this.coloredInnerMask.GetComponent<Mask>() == null)
            {
                this.coloredInnerMask.gameObject.AddComponent<Mask>().showMaskGraphic = false;
            }
            component = this.coloredInnerMask.GetComponent<Image>();
            if (component == null)
            {
                component = this.coloredInnerMask.gameObject.AddComponent<Image>();
            }
            component.fillMethod = Image.FillMethod.Horizontal;
            component.fillOrigin = 1;
            component.type = Image.Type.Filled;
            this.Stretch(this.coloredInnerMask);
            this.CreateIfAbsent(ref this.coloredFiller, this.coloredInnerMask, "Filler");
            if (this.coloredFiller.GetComponent<Image>() == null)
            {
                this.coloredFiller.gameObject.AddComponent<Image>().color = Color.green;
            }
            this.Stretch(this.coloredFiller);
        }

        private void Stretch(RectTransform child)
        {
            child.anchorMax = new Vector2(1f, 1f);
            child.anchorMin = new Vector2(0f, 0f);
            child.anchoredPosition = Vector2.zero;
            child.sizeDelta = Vector2.zero;
        }

        public float InitialProgress
        {
            get => 
                this.initialProgress;
            set
            {
                this.initialProgress = value;
                this.initialMaskImage.fillAmount = this.initialProgress;
            }
        }

        public float ColoredProgress
        {
            get => 
                this.coloredProgress;
            set
            {
                this.coloredProgress = value;
                this.coloredMaskImage.fillAmount = this.coloredProgress;
                this.coloredInnerMaskImage.fillAmount = 1f - this.initialProgress;
            }
        }
    }
}

