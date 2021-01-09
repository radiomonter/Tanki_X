namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class HUDBar : MonoBehaviour
    {
        private readonly List<Ruler> rulers = new List<Ruler>();
        protected float currentValue;
        protected float maxValue;

        protected float Clamp(float value) => 
            Mathf.Max(0f, Mathf.Min(value, this.maxValue));

        protected virtual void OnMaxValueChanged()
        {
        }

        protected void UpdateSegments()
        {
            for (int i = 0; i < this.Rulers.Count; i++)
            {
                this.Rulers[i].segmentsCount = (int) (this.maxValue / this.AmountPerSegment);
                this.Rulers[i].UpdateSegments();
            }
        }

        protected List<Ruler> Rulers
        {
            get
            {
                if (this.rulers.Count == 0)
                {
                    base.GetComponentsInChildren<Ruler>(true, this.rulers);
                }
                return this.rulers;
            }
        }

        public float MaxValue
        {
            get => 
                this.maxValue;
            set
            {
                if (this.maxValue != value)
                {
                    this.maxValue = value;
                    this.UpdateSegments();
                    this.OnMaxValueChanged();
                }
            }
        }

        public virtual float CurrentValue
        {
            get => 
                this.currentValue;
            set => 
                this.currentValue = value;
        }

        public virtual float AmountPerSegment
        {
            get => 
                0f;
            set
            {
            }
        }
    }
}

