namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class UIRectClipper : MonoBehaviour, IClipper
    {
        [HideInInspector, SerializeField]
        private float fromX;
        [HideInInspector, SerializeField]
        private float toX = 1f;
        [SerializeField]
        private UnityEngine.RectTransform.Axis axis;
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
            UnityEngine.RectTransform.Axis axis = this.axis;
            if (axis == UnityEngine.RectTransform.Axis.Horizontal)
            {
                float width = canvasRect.width;
                canvasRect.xMin = Mathf.Max(canvasRect.xMin, canvasRect.xMin + (this.FromX * width));
                canvasRect.xMax = Mathf.Min(canvasRect.xMax, canvasRect.xMax - ((1f - this.ToX) * width));
            }
            else if (axis == UnityEngine.RectTransform.Axis.Vertical)
            {
                float height = canvasRect.height;
                canvasRect.yMin = Mathf.Max(canvasRect.yMin, canvasRect.yMin + (this.FromX * height));
                canvasRect.yMax = Mathf.Min(canvasRect.yMax, canvasRect.yMax - ((1f - this.ToX) * height));
            }
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

        public float FromX
        {
            get => 
                this.fromX;
            set
            {
                this.fromX = value;
                this.PerformClipping();
            }
        }

        public float ToX
        {
            get => 
                this.toX;
            set
            {
                this.toX = value;
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

