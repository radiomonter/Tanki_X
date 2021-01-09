namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SimpleLayoutCalculator
    {
        private static readonly float EPSILON = 0.001f;
        public List<Element> elements = new List<Element>();
        private int elementIndex;
        public float totalMin;
        public float totalFlexible;
        public float totalMax;
        public bool unlimited;
        public int iterations;
        public int iterationsOuter;
        private float sizeLeft;
        private List<Element> unresolvedElements = new List<Element>();
        public int newCount;

        public void AddElement(float flexible = 0f, float min = 0f, float max = 0f)
        {
            this.EnsureArraySize(this.elementIndex + 1);
            Element item = this.elements[this.elementIndex];
            item.i = this.elementIndex;
            item.Min = min;
            item.Flexible = flexible;
            item.Max = max;
            item.size = 0f;
            item.sizeIfNoLimits = 0f;
            this.totalMin += item.Min;
            this.totalFlexible += item.Flexible;
            this.totalMax += item.Max;
            this.unlimited |= item.Unlimited();
            if (!item.IsFixed())
            {
                this.unresolvedElements.Add(item);
            }
            this.elementIndex++;
        }

        public void Calculate(float maxSize)
        {
            this.SetArraySize(this.elementIndex);
            this.sizeLeft = !this.unlimited ? Mathf.Min(this.totalMax, maxSize) : Mathf.Max(this.totalMin, maxSize);
            this.DistributeSizeToFixedElements();
            if (this.totalFlexible != 0f)
            {
                while (true)
                {
                    if (!this.DistributedAll())
                    {
                        this.iterationsOuter++;
                        this.DistributeSizeLeft();
                        this.ClampMinMax();
                        if (!this.DistributedAll())
                        {
                            if (this.sizeLeft > 0f)
                            {
                                this.ResolveElementsAtMax();
                            }
                            else
                            {
                                this.ResolveElementsAtMin();
                            }
                            this.RevertSizeFromUnresolvedElements();
                        }
                        if ((this.iterations < 100) && (this.unresolvedElements.Count != 0))
                        {
                            continue;
                        }
                    }
                    return;
                }
            }
        }

        private void ClampMinMax()
        {
            int num = 0;
            while (num < this.unresolvedElements.Count)
            {
                Element element = this.unresolvedElements[num];
                float size = element.size;
                if (element.sizeIfNoLimits < element.Min)
                {
                    element.size = element.Min;
                }
                else if ((element.Max > 0f) && (element.sizeIfNoLimits > element.Max))
                {
                    element.size = element.Max;
                }
                this.sizeLeft -= element.size - size;
                num++;
                this.iterations++;
            }
        }

        private bool DistributedAll() => 
            (-EPSILON < this.sizeLeft) && (this.sizeLeft < EPSILON);

        private void DistributeSizeLeft()
        {
            float sizeLeft = this.sizeLeft;
            int num2 = 0;
            while (num2 < this.unresolvedElements.Count)
            {
                Element element = this.unresolvedElements[num2];
                element.sizeIfNoLimits = (sizeLeft * element.Flexible) / this.totalFlexible;
                float size = element.size;
                element.size = element.sizeIfNoLimits;
                this.sizeLeft -= element.size - size;
                num2++;
                this.iterations++;
            }
        }

        private void DistributeSizeToFixedElements()
        {
            int num = 0;
            while (num < this.elements.Count)
            {
                Element element = this.elements[num];
                if (element.IsFixed())
                {
                    float min = element.Min;
                    element.size = min;
                    this.sizeLeft -= min;
                }
                num++;
                this.iterations++;
            }
        }

        private void EnsureArraySize(int count)
        {
            while (this.elements.Count < count)
            {
                this.elements.Add(new Element());
                this.newCount++;
            }
        }

        public void Reset(int count)
        {
            this.newCount = 0;
            this.EnsureArraySize(count);
            this.elementIndex = 0;
            this.totalMin = 0f;
            this.totalFlexible = 0f;
            this.totalMax = 0f;
            this.unlimited = false;
            this.iterations = 0;
            this.iterationsOuter = 0;
            this.unresolvedElements.Clear();
            this.unresolvedElements.Capacity = count;
        }

        private void ResolveElementsAtMax()
        {
            int index = 0;
            while (index < this.unresolvedElements.Count)
            {
                Element element = this.unresolvedElements[index];
                if (element.AtMax())
                {
                    this.unresolvedElements.RemoveAt(index);
                    this.totalFlexible -= element.Flexible;
                    index--;
                }
                index++;
                this.iterations++;
            }
        }

        private void ResolveElementsAtMin()
        {
            int index = 0;
            while (index < this.unresolvedElements.Count)
            {
                Element element = this.unresolvedElements[index];
                if (element.AtMin())
                {
                    this.unresolvedElements.RemoveAt(index);
                    this.totalFlexible -= element.Flexible;
                    index--;
                }
                index++;
                this.iterations++;
            }
        }

        private void RevertSizeFromUnresolvedElements()
        {
            int num = 0;
            while (num < this.unresolvedElements.Count)
            {
                Element element = this.unresolvedElements[num];
                this.sizeLeft += element.size;
                element.size = 0f;
                num++;
                this.iterations++;
            }
        }

        private void SetArraySize(int count)
        {
            this.EnsureArraySize(count);
            while (this.elements.Count > count)
            {
                this.elements.RemoveAt(this.elements.Count - 1);
            }
        }

        public class Element
        {
            public float Min;
            public float Flexible;
            public float Max;
            public float size;
            public float i;
            public float sizeIfNoLimits;

            public bool AtMax() => 
                (this.Max > 0f) && (this.size == this.Max);

            public bool AtMin() => 
                this.size == this.Min;

            public bool IsFixed() => 
                this.Flexible == 0f;

            public override string ToString()
            {
                object[] objArray1 = new object[] { "[i=", this.i, " sizeIfNoLimits=", this.sizeIfNoLimits, $" Min: {this.Min}, Flexible: {this.Flexible}, Max: {this.Max}, Size: {this.size}]" };
                return string.Concat(objArray1);
            }

            public bool Unlimited() => 
                (this.Flexible > 0f) && (this.Max == 0f);
        }
    }
}

