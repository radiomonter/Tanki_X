namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class SimpleLayoutGroup : LayoutGroup
    {
        [SerializeField]
        protected float m_Spacing;
        private SimpleLayoutCalculator calculator = new SimpleLayoutCalculator();

        protected SimpleLayoutGroup()
        {
        }

        protected void CalcAlongAxis(int axis, bool isVertical)
        {
            float num = (axis != 0) ? ((float) base.padding.vertical) : ((float) base.padding.horizontal);
            float b = num;
            float num3 = 0f;
            bool flag = isVertical ^ (axis == 1);
            for (int i = 0; i < base.rectChildren.Count; i++)
            {
                RectTransform rect = base.rectChildren[i];
                float flexibleSize = SimpleLayoutUtility.GetFlexibleSize(rect, axis);
                float minSize = SimpleLayoutUtility.GetMinSize(rect, axis);
                if (flag)
                {
                    num3 = Mathf.Max(flexibleSize, num3);
                    b = Mathf.Max(minSize + num, b);
                }
                else
                {
                    num3 += flexibleSize;
                    b += minSize + this.spacing;
                }
            }
            if (!flag && (base.rectChildren.Count > 0))
            {
                b -= this.spacing;
            }
            base.SetLayoutInputForAxis(b, b, num3, axis);
        }

        protected void SetChildrenAlongAxis(int axis, bool isVertical)
        {
            float num = base.rectTransform.rect.size[axis];
            if (isVertical ^ (axis == 1))
            {
                float num2 = num - ((axis != 0) ? ((float) base.padding.vertical) : ((float) base.padding.horizontal));
                for (int i = 0; i < base.rectChildren.Count; i++)
                {
                    RectTransform rect = base.rectChildren[i];
                    float flexibleSize = SimpleLayoutUtility.GetFlexibleSize(rect, axis);
                    float minSize = SimpleLayoutUtility.GetMinSize(rect, axis);
                    float maxSize = SimpleLayoutUtility.GetMaxSize(rect, axis);
                    float num7 = minSize;
                    float a = Mathf.Clamp(num2, minSize, (flexibleSize <= 0f) ? num7 : num);
                    if (maxSize > 0f)
                    {
                        a = Mathf.Min(a, maxSize);
                    }
                    float startOffset = base.GetStartOffset(axis, a);
                    base.SetChildAlongAxis(rect, axis, startOffset, a);
                }
            }
            else
            {
                float pos = (axis != 0) ? ((float) base.padding.top) : ((float) base.padding.left);
                if ((base.GetTotalFlexibleSize(axis) == 0f) && (base.GetTotalPreferredSize(axis) < num))
                {
                    pos = this.GetStartOffset(axis, base.GetTotalPreferredSize(axis) - ((axis != 0) ? ((float) base.padding.vertical) : ((float) base.padding.horizontal)));
                }
                this.calculator.Reset(base.rectChildren.Count);
                int num11 = 0;
                while (true)
                {
                    if (num11 >= base.rectChildren.Count)
                    {
                        this.calculator.Calculate(num - ((base.rectChildren.Count <= 1) ? 0f : ((base.rectChildren.Count - 1) * this.spacing)));
                        for (int i = 0; i < base.rectChildren.Count; i++)
                        {
                            RectTransform transform3 = base.rectChildren[i];
                            SimpleLayoutCalculator.Element element = this.calculator.elements[i];
                            base.SetChildAlongAxis(transform3, axis, pos, element.size);
                            pos += element.size + this.spacing;
                        }
                        break;
                    }
                    RectTransform rect = base.rectChildren[num11];
                    float flexibleSize = SimpleLayoutUtility.GetFlexibleSize(rect, axis);
                    float minSize = SimpleLayoutUtility.GetMinSize(rect, axis);
                    float maxSize = SimpleLayoutUtility.GetMaxSize(rect, axis);
                    this.calculator.AddElement(flexibleSize, minSize, maxSize);
                    num11++;
                }
            }
        }

        public float spacing
        {
            get => 
                this.m_Spacing;
            set => 
                base.SetProperty<float>(ref this.m_Spacing, value);
        }
    }
}

