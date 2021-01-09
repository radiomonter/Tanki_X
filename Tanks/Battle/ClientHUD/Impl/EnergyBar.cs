namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class EnergyBar : HUDBar
    {
        [SerializeField]
        private Ruler stroke;
        [SerializeField]
        private Ruler fill;
        [SerializeField]
        private Ruler glow;
        [SerializeField]
        private Ruler energyInjectionGlow;
        [SerializeField]
        private TankPartItemIcon turretIcon;
        private bool isBlinking;
        private float amountPerSegment = 1f;

        private void ApplyFill()
        {
            this.fill.FillAmount = base.currentValue / base.maxValue;
            this.stroke.RectTransform.anchoredPosition = new Vector2(this.fill.RectTransform.rect.width * this.fill.FillAmount, this.stroke.RectTransform.anchoredPosition.y);
            this.stroke.FillAmount = 1f - this.fill.FillAmount;
            this.glow.FillAmount = this.fill.FillAmount;
            this.energyInjectionGlow.FillAmount = this.fill.FillAmount;
        }

        public void Blink(bool canShoot)
        {
            Animator component = base.GetComponent<Animator>();
            if (component.isActiveAndEnabled)
            {
                component.SetBool("CanShoot", canShoot);
                component.SetTrigger("Blink");
            }
        }

        [DebuggerHidden]
        private IEnumerator BlinkCoroutine(bool canShoot) => 
            new <BlinkCoroutine>c__Iterator0 { 
                canShoot = canShoot,
                $this = this
            };

        public void EnergyInjectionBlink(bool canShoot)
        {
            Animator component = base.GetComponent<Animator>();
            if (component.isActiveAndEnabled)
            {
                component.SetBool("CanShoot", canShoot);
                component.SetTrigger("EnergyInjectionBlink");
            }
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

        public long TurretId
        {
            set => 
                this.turretIcon.SetIconWithName(value.ToString());
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
            internal EnergyBar $this;
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

