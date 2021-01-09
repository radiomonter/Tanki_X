namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class EnergyBarGlow : HUDBar
    {
        [SerializeField]
        private Ruler fill;
        [SerializeField]
        private Ruler glow;
        [SerializeField]
        private Ruler energyInjectionGlow;
        [SerializeField]
        private BarFillEnd barFillEnd;
        private bool isBlinking;
        private float amountPerSegment = 1f;

        private void ApplyFill()
        {
            this.fill.FillAmount = base.currentValue / base.maxValue;
            this.glow.FillAmount = this.fill.FillAmount;
            this.energyInjectionGlow.FillAmount = this.fill.FillAmount;
            this.barFillEnd.FillAmount = this.fill.FillAmount;
        }

        public void Blink(bool canShoot)
        {
            base.GetComponent<Animator>().SetBool("CanShoot", canShoot);
            base.GetComponent<Animator>().SetTrigger("Blink");
        }

        [DebuggerHidden]
        private IEnumerator BlinkCoroutine(bool canShoot) => 
            new <BlinkCoroutine>c__Iterator0 { 
                canShoot = canShoot,
                $this = this
            };

        public void EnergyInjectionBlink(bool canShoot)
        {
            base.GetComponent<Animator>().SetBool("CanShoot", canShoot);
            base.GetComponent<Animator>().SetTrigger("EnergyInjectionBlink");
        }

        private void OnDisable()
        {
            this.StopBlinking();
        }

        public void StartBlinking(bool canShoot)
        {
            if (!this.isBlinking)
            {
                base.StartCoroutine(this.BlinkCoroutine(canShoot));
            }
        }

        public void StopBlinking()
        {
            this.isBlinking = false;
        }

        public override float CurrentValue
        {
            get => 
                base.currentValue;
            set
            {
                base.currentValue = base.Clamp(value);
                this.ApplyFill();
            }
        }

        public override float AmountPerSegment
        {
            get => 
                this.amountPerSegment;
            set
            {
                if (this.amountPerSegment != value)
                {
                    this.amountPerSegment = value;
                    base.UpdateSegments();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <BlinkCoroutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal bool canShoot;
            internal EnergyBarGlow $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$this.isBlinking = true;
                        break;

                    case 1:
                        break;

                    default:
                        goto TR_0000;
                }
                if (this.$this.isBlinking)
                {
                    this.$this.Blink(this.canShoot);
                    this.$current = new WaitForSeconds(0.5f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$PC = -1;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

