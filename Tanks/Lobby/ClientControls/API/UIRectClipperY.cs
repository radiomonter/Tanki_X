namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class UIRectClipperY : MonoBehaviour, IClipper
    {
        [HideInInspector, SerializeField]
        private float fromY;
        [HideInInspector, SerializeField]
        private float toY = 1f;
        private UnityEngine.RectTransform rectTransform;
        private List<MaskableGraphic> maskableCache = new List<MaskableGraphic>();
        private readonly List<Canvas> canvases = new List<Canvas>();
        private readonly Vector3[] worldCorners = new Vector3[4];
        private readonly Vector3[] canvasCorners = new Vector3[4];

        public Rect GetCanvasRect()
        {
            Canvas canvas = null;
            base.gameObject.GetComponentsInParent<Canvas>(false, this.canvases);
            if (this.canvases.Count <= 0)
            {
                return new Rect();
            }
            canvas = this.canvases[this.canvases.Count - 1];
            this.canvases.Clear();
            this.RectTransform.GetWorldCorners(this.worldCorners);
            Transform transform = canvas.transform;
            for (int i = 0; i < 4; i++)
            {
                this.canvasCorners[i] = transform.InverseTransformPoint(this.worldCorners[i]);
            }
            return new Rect(this.canvasCorners[0].x, this.canvasCorners[0].y, this.canvasCorners[2].x - this.canvasCorners[0].x, this.canvasCorners[2].y - this.canvasCorners[0].y);
        }

        private void OnDisable()
        {
            ClipperRegistry.Unregister(this);
        }

        private void OnEnable()
        {
            ClipperRegistry.Register(this);
        }

        public void PerformClipping()
        {
            Rect canvasRect = this.GetCanvasRect();
            float height = canvasRect.height;
            canvasRect.yMin = Mathf.Max(canvasRect.yMin, canvasRect.yMin + (this.FromY * height));
            canvasRect.yMax = Mathf.Min(canvasRect.yMax, canvasRect.yMax - ((1f - this.ToY) * height));
            CanvasRenderer component = base.GetComponent<CanvasRenderer>();
            if (component != null)
            {
                component.EnableRectClipping(canvasRect);
            }
            this.maskableCache.Clear();
            base.GetComponentsInChildren<MaskableGraphic>(this.maskableCache);
            foreach (MaskableGraphic graphic in this.maskableCache)
            {
                graphic.SetClipRect(canvasRect, true);
            }
        }

        public float FromY
        {
            get => 
                this.fromY;
            set
            {
                this.fromY = value;
                this.PerformClipping();
            }
        }

        public float ToY
        {
            get => 
                this.toY;
            set
            {
                this.toY = value;
                this.PerformClipping();
            }
        }

        private UnityEngine.RectTransform RectTransform
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
    }
}

