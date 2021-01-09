namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [ExecuteInEditMode]
    public class ContentSize : UIBehaviour
    {
        public Vector2 offsets;
        public bool constantWidth;
        private Rect lastCanvasRect;
        private RectTransform canvasRectTransform;

        protected override void OnEnable()
        {
            this.Validate();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            this.Validate();
        }

        protected override void OnTransformParentChanged()
        {
            this.canvasRectTransform = null;
        }

        private void Update()
        {
            if (this.lastCanvasRect != this.CanvasRectTransform.rect)
            {
                this.lastCanvasRect = this.CanvasRectTransform.rect;
                this.Validate();
            }
        }

        private void Validate()
        {
            RectTransform canvasRectTransform = this.CanvasRectTransform;
            if (canvasRectTransform != null)
            {
                RectTransform component = base.GetComponent<RectTransform>();
                Vector2 sizeDelta = component.sizeDelta;
                if (!this.constantWidth)
                {
                    sizeDelta.x = canvasRectTransform.rect.width - this.offsets.x;
                }
                sizeDelta.y = canvasRectTransform.rect.height - this.offsets.y;
                component.sizeDelta = sizeDelta;
            }
        }

        private RectTransform CanvasRectTransform
        {
            get
            {
                if (this.canvasRectTransform == null)
                {
                    Canvas componentInParent = base.GetComponentInParent<Canvas>();
                    if (componentInParent != null)
                    {
                        this.canvasRectTransform = componentInParent.GetComponent<RectTransform>();
                    }
                }
                return this.canvasRectTransform;
            }
        }
    }
}

