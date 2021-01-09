namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class HPBarGlow : HUDBar
    {
        [SerializeField]
        private Image fill;
        [SerializeField]
        private Image diff;
        [SerializeField]
        private TextMeshProUGUI hpValues;
        [SerializeField]
        private HPBarFillEnd hpBarFillEnd;
        private float deltaHP;

        public void Change(float delta)
        {
            this.deltaHP = delta;
            this.deltaHP = base.Clamp(base.currentValue + this.deltaHP) - base.currentValue;
            float fillAmount = Mathf.Min(base.currentValue + this.deltaHP, base.currentValue) / base.maxValue;
            float num4 = Mathf.Max(base.currentValue + this.deltaHP, base.currentValue) / base.maxValue;
            this.SetFillAmount(this.fill, fillAmount);
            this.SetFillAmount(this.diff, num4);
            this.hpBarFillEnd.FillAmount = num4;
            Animator component = base.GetComponent<Animator>();
            if (component.isActiveAndEnabled)
            {
                component.SetFloat("Fill", fillAmount);
                component.SetInteger("Diff", (int) this.deltaHP);
                component.SetTrigger("Start");
            }
            this.fill.color = (fillAmount <= 0.2f) ? ((Color) new Color32(0xff, 0x3b, 0x3b, 0xff)) : ((Color) new Color32(0xa8, 0xff, 0x2f, 0xff));
            this.UpdateHPValues((int) (base.currentValue + Mathf.Max(0f, this.deltaHP)));
        }

        protected override void OnMaxValueChanged()
        {
            this.ResetDiff();
        }

        public void ResetDiff()
        {
            base.currentValue = base.Clamp(base.currentValue + this.deltaHP);
            this.deltaHP = 0f;
            float fillAmount = base.currentValue / base.maxValue;
            this.SetFillAmount(this.fill, fillAmount);
            this.hpBarFillEnd.FillAmount = fillAmount;
            this.fill.color = (fillAmount <= 0.2f) ? ((Color) new Color32(0xff, 0x3b, 0x3b, 0xff)) : ((Color) new Color32(0xa8, 0xff, 0x2f, 0xff));
            Animator component = base.GetComponent<Animator>();
            if (component.isActiveAndEnabled)
            {
                component.SetInteger("Diff", 0);
                component.SetFloat("Fill", fillAmount);
            }
            this.UpdateHPValues((int) base.currentValue);
        }

        private void SetFillAmount(Image image, float fillAmount)
        {
            image.fillAmount = fillAmount;
        }

        private void UpdateHPValues(int value)
        {
            this.hpValues.text = $"{value}/<size=16>{(int) base.maxValue}";
        }

        public float Diff =>
            this.deltaHP;

        public override float CurrentValue
        {
            get => 
                base.currentValue;
            set => 
                this.Change(value - base.currentValue);
        }

        public override float AmountPerSegment =>
            base.maxValue;
    }
}

