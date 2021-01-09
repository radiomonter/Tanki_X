namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class Ruler : MonoBehaviour
    {
        [SerializeField]
        private Image segment;
        [SerializeField]
        private float spacing = 2f;
        public int segmentsCount = 1;
        public List<Image> segments = new List<Image>();
        private UnityEngine.RectTransform rectTransform;
        private float fillAmount;
        public UnityEngine.Color Color;
        private UnityEngine.Color imageColor;

        public void Clear()
        {
            this.imageColor = UnityEngine.Color.clear;
            foreach (Image image in this.segments)
            {
                if (image != null)
                {
                    DestroyImmediate(image.gameObject);
                }
            }
            this.segments.Clear();
        }

        private float GetSegmentAnchorMax(int i)
        {
            float width = this.RectTransform.rect.width;
            float num3 = (this.spacing / width) / 2f;
            return ((((width / ((float) this.segmentsCount)) / width) * (i + 1)) - ((i >= (this.segmentsCount - 1)) ? 0f : num3));
        }

        private float GetSegmentAnchorMin(int i)
        {
            float width = this.RectTransform.rect.width;
            float num3 = (this.spacing / width) / 2f;
            return ((((width / ((float) this.segmentsCount)) / width) * i) + ((i <= 0) ? 0f : num3));
        }

        private void Update()
        {
            if (this.imageColor != this.Color)
            {
                for (int i = 0; i < this.segments.Count; i++)
                {
                    this.segments[i].color = this.imageColor = this.Color;
                }
            }
        }

        public void UpdateSegments()
        {
            this.Clear();
            for (int i = 0; i < this.segmentsCount; i++)
            {
                Image item = Instantiate<Image>(this.segment, base.transform);
                item.rectTransform.anchorMin = new Vector2(this.GetSegmentAnchorMin(i), 0f);
                item.rectTransform.anchorMax = new Vector2(this.GetSegmentAnchorMax(i), 1f);
                Vector2 zero = Vector2.zero;
                item.rectTransform.offsetMax = zero;
                item.rectTransform.offsetMin = zero;
                item.gameObject.SetActive(true);
                this.segments.Add(item);
            }
        }

        public float Spacing
        {
            get => 
                this.spacing;
            set => 
                this.spacing = value;
        }

        public UnityEngine.RectTransform RectTransform
        {
            get
            {
                if (this.rectTransform == null)
                {
                    this.rectTransform = base.GetComponent<UnityEngine.RectTransform>();
                }
                return this.rectTransform;
            }
        }

        public float FillAmount
        {
            get => 
                this.fillAmount;
            set
            {
                this.fillAmount = value;
                float num = 1f / ((float) this.segments.Count);
                for (int i = 0; i < this.segments.Count; i++)
                {
                    float t = Mathf.Clamp01((value - (i * num)) / num);
                    float segmentAnchorMin = this.GetSegmentAnchorMin(i);
                    float segmentAnchorMax = this.GetSegmentAnchorMax(i);
                    this.segments[i].rectTransform.anchorMax = new Vector2(Mathf.Lerp(segmentAnchorMin, segmentAnchorMax, t), 1f);
                }
            }
        }
    }
}

