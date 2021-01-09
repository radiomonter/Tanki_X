namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class UserMoneyIndicatorComponent : BehaviourComponent
    {
        [SerializeField]
        private Text moneyText;
        private const float setMoneyAnimationSpeedPerSec = 100f;
        private const float setMoneyAnimationMaxTime = 5f;
        private UnityEngine.Animator animator;
        private long money;
        private long moneySuspended;
        private long moneyExpected;

        private void ApplyMoney()
        {
            this.moneyText.text = ((long) (this.money - this.moneySuspended)).ToStringSeparatedByThousands();
        }

        public void SetMoneyAnimated(long value)
        {
            if ((this.moneyExpected > 0L) && !this.money.Equals(this.moneyExpected))
            {
                base.StopAllCoroutines();
                this.money = this.moneyExpected;
                this.ApplyMoney();
            }
            this.moneyExpected = value;
            base.StartCoroutine(this.ShowAnimation(value));
        }

        public void SetMoneyImmediately(long value)
        {
            this.money = value;
            this.ApplyMoney();
        }

        [DebuggerHidden]
        private IEnumerator ShowAnimation(long newMoneyValue) => 
            new <ShowAnimation>c__Iterator0 { 
                newMoneyValue = newMoneyValue,
                $this = this
            };

        public void Suspend(long value)
        {
            this.moneySuspended = value;
            this.ApplyMoney();
        }

        private UnityEngine.Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    this.animator = base.GetComponent<UnityEngine.Animator>();
                }
                return this.animator;
            }
        }

        [CompilerGenerated]
        private sealed class <ShowAnimation>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal long newMoneyValue;
            internal float <moneyDiff>__0;
            internal float <setMoneyAnimationTime>__0;
            internal long <moneyDiffSign>__0;
            internal float <delay>__0;
            internal int <step>__0;
            internal UserMoneyIndicatorComponent $this;
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
                        this.<moneyDiff>__0 = this.newMoneyValue - this.$this.money;
                        this.<setMoneyAnimationTime>__0 = Mathf.Min((float) (Mathf.Abs(this.<moneyDiff>__0) / 100f), (float) 5f);
                        this.<moneyDiffSign>__0 = (long) Mathf.Sign(this.<moneyDiff>__0);
                        this.<delay>__0 = this.<setMoneyAnimationTime>__0 / Mathf.Abs(this.<moneyDiff>__0);
                        this.<step>__0 = Mathf.CeilToInt(0.02f / this.<delay>__0);
                        break;

                    case 1:
                        this.$this.money += this.<moneyDiffSign>__0 * this.<step>__0;
                        this.$this.ApplyMoney();
                        break;

                    case 2:
                        this.$this.money = this.newMoneyValue;
                        this.$this.ApplyMoney();
                        this.$this.Animator.SetTrigger("flash");
                        this.$PC = -1;
                        goto TR_0000;

                    default:
                        goto TR_0000;
                }
                if (Mathf.Abs((float) (this.$this.money - this.newMoneyValue)) <= this.<step>__0)
                {
                    this.$current = new WaitForSeconds(this.<delay>__0);
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                }
                else
                {
                    this.$current = new WaitForSeconds(this.<delay>__0);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                }
                return true;
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

