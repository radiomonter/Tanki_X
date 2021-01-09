namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Animator))]
    public class HPBar : HUDBar
    {
        [SerializeField]
        private Image fill;
        [SerializeField]
        private Image fillUnderDiff;
        [SerializeField]
        private Image diff;
        [SerializeField]
        private TankPartItemIcon hullIcon;
        [SerializeField]
        private TextMeshProUGUI hpValues;
        private float deltaHP;

        public void Change(float delta)
        {
            this.deltaHP = delta;
            this.deltaHP = base.Clamp(base.currentValue + this.deltaHP) - base.currentValue;
            Animator component = base.GetComponent<Animator>();
            if (component.isActiveAndEnabled)
            {
                component.SetInteger("Diff", (int) this.deltaHP);
                component.SetTrigger("Start");
            }
            float num = Mathf.Max(base.currentValue + this.deltaHP, base.currentValue);
            float fillAmount = Mathf.Min(base.currentValue + this.deltaHP, base.currentValue) / base.maxValue;
            this.SetFillAmount(this.fill, fillAmount);
            this.fill.color = (fillAmount <= 0.2f) ? ((Color) new Color32(0xff, 0x3b, 0x3b, 0xff)) : ((Color) new Color32(0xa8, 0xff, 0x2f, 0xff));
            float num4 = (num / base.maxValue) - fillAmount;
            this.SetFillAmount(this.diff, num4);
            float width = ((RectTransform) base.transform).rect.width;
            this.diff.rectTransform.anchoredPosition = new Vector2(width * fillAmount, this.diff.rectTransform.anchoredPosition.y);
            this.fillUnderDiff.rectTransform.anchoredPosition = new Vector2(width * fillAmount, this.fillUnderDiff.rectTransform.anchoredPosition.y);
            this.SetFillAmount(this.fillUnderDiff, num4 - fillAmount);
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
            this.fill.color = (fillAmount <= 0.2f) ? ((Color) new Color32(0xff, 0x3b, 0x3b, 0xff)) : ((Color) new Color32(0xa8, 0xff, 0x2f, 0xff));
            this.UpdateHPValues((int) base.currentValue);
        }

        private void SetFillAmount(Image image, float fillAmount)
        {
            image.rectTransform.anchorMax = new Vector2(fillAmount, 1f);
        }

        private void UpdateHPValues(int value)
        {
            this.hpValues.text = $"{value}/<size=16>{(int) base.maxValue}";
        }

        public long HullId
        {
            set => 
                this.hullIcon.SetIconWithName(value.ToString());
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

