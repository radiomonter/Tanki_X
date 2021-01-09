namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class DependentSize : ResizeListener, ILayoutElement
    {
        public bool useMinWidth;
        public bool usePreferredWidth;
        public bool useFlexibleWidth;
        public bool useMinHeight;
        public bool usePreferredHeight;
        public bool useFlexibleHeight;
        private float calculatedMinWidth;
        private float calculatedMinHeight;
        private float calculatedPreferredWidth;
        private float calculatedPreferredHeight;
        private float calculatedFlexibleWidth;
        private float calculatedFlexibleHeight;
        private RectTransform layoutSource;

        public void CalculateLayoutInputHorizontal()
        {
            this.calculatedMinWidth = this.GetValue(this.useMinWidth, () => LayoutUtility.GetMinWidth(this.layoutSource));
            this.calculatedFlexibleWidth = this.GetValue(this.useFlexibleWidth, () => LayoutUtility.GetFlexibleWidth(this.layoutSource));
            this.calculatedPreferredWidth = this.GetValue(this.usePreferredWidth, () => LayoutUtility.GetPreferredWidth(this.layoutSource));
        }

        public void CalculateLayoutInputVertical()
        {
            this.calculatedMinHeight = this.GetValue(this.useMinHeight, () => LayoutUtility.GetMinHeight(this.layoutSource));
            this.calculatedFlexibleHeight = this.GetValue(this.useFlexibleHeight, () => LayoutUtility.GetFlexibleHeight(this.layoutSource));
            this.calculatedPreferredHeight = this.GetValue(this.usePreferredHeight, () => LayoutUtility.GetPreferredHeight(this.layoutSource));
        }

        private float GetValue(bool use, Func<float> value) => 
            (!use || (this.layoutSource == null)) ? -1f : value();

        public override void OnResize(RectTransform source)
        {
            this.layoutSource = source;
            this.CalculateLayoutInputHorizontal();
            this.CalculateLayoutInputVertical();
            LayoutRebuilder.MarkLayoutForRebuild(base.GetComponent<RectTransform>());
        }

        public float minWidth =>
            this.calculatedMinWidth;

        public float preferredWidth =>
            this.calculatedPreferredWidth;

        public float flexibleWidth =>
            this.calculatedFlexibleWidth;

        public float minHeight =>
            this.calculatedMinHeight;

        public float preferredHeight =>
            this.calculatedPreferredHeight;

        public float flexibleHeight =>
            this.calculatedFlexibleHeight;

        public int layoutPriority =>
            0;
    }
}

