namespace Tanks.Battle.ClientBattleSelect.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class EnergyResultUI : MonoBehaviour
    {
        [SerializeField]
        private List<AnimatedDiffProgress> charges;
        [SerializeField]
        protected TextMeshProUGUI earnedEnergyText;
        [SerializeField, FormerlySerializedAs("compensationText")]
        protected TextMeshProUGUI compensationCrystalsText;
        [SerializeField, FormerlySerializedAs("compensationObject")]
        protected GameObject compensationCrystalsObject;
        [SerializeField, FormerlySerializedAs("cashbackText")]
        protected TextMeshProUGUI mvpCashbackTextObject;
        [SerializeField]
        protected TextMeshProUGUI chargesFullText;
        [SerializeField]
        private TooltipShowBehaviour energyBarTooltip;
        [SerializeField]
        private TooltipShowBehaviour mvpBonusTooltip;
        [SerializeField]
        private TooltipShowBehaviour unfairBonusTooltip;
        [SerializeField, FormerlySerializedAs("earnedTextTemplate")]
        private LocalizedField cashbackText;
        [SerializeField, FormerlySerializedAs("cashbackEnergyTextTemplate")]
        private LocalizedField mvpCashbackText;
        [SerializeField]
        private LocalizedField unfairMatchText;
        [SerializeField]
        private float duration = 0.3f;
        private List<float> previousProgress;
        private long currentEnergy;
        private long energyInCharge;
        private int earnedEnergy;
        [SerializeField]
        private Color fullColor = ((Color) new Color32(0x84, 0xf6, 0xf6, 0xff));
        [SerializeField]
        private Color partColor = ((Color) new Color32(0x80, 0x80, 0x80, 0xff));

        public void SetEnergyCompensation(int compensationCrystals, bool mvpCashback, bool isUnfairCashback)
        {
            if (compensationCrystals <= 0)
            {
                this.earnedEnergyText.text = string.Format(!mvpCashback ? ((string) this.cashbackText) : ((string) this.mvpCashbackText), this.earnedEnergy);
                this.earnedEnergyText.gameObject.SetActive(this.earnedEnergy > 0);
                this.compensationCrystalsObject.SetActive(false);
                this.chargesFullText.gameObject.SetActive(false);
            }
            else
            {
                this.earnedEnergyText.gameObject.SetActive(false);
                this.compensationCrystalsText.text = string.Format((string) this.cashbackText, compensationCrystals);
                this.compensationCrystalsObject.SetActive(true);
                this.mvpCashbackTextObject.gameObject.SetActive(mvpCashback);
                this.chargesFullText.gameObject.SetActive(true);
            }
            this.mvpBonusTooltip.gameObject.SetActive(mvpCashback);
            this.unfairBonusTooltip.gameObject.SetActive(isUnfairCashback);
        }

        public void SetEnergyResult(long currentEnergy, long energyInCharge, int earnedEnergy)
        {
            this.previousProgress = new List<float>(new float[this.charges.Count]);
            long num = currentEnergy / energyInCharge;
            this.currentEnergy = currentEnergy;
            this.energyInCharge = energyInCharge;
            this.earnedEnergy = earnedEnergy;
            long num2 = currentEnergy - earnedEnergy;
            for (int i = 0; i < this.charges.Count; i++)
            {
                float num4 = Mathf.Clamp01(((float) (num2 - (i * energyInCharge))) / ((float) energyInCharge));
                this.previousProgress[i] = num4;
                this.charges[i].Set(num4, num4);
                if (i < num)
                {
                    this.charges[i].FillImage.color = this.fullColor;
                    this.charges[i].DiffImage.color = this.fullColor;
                }
                else if (i == num)
                {
                    this.charges[i].FillImage.color = this.partColor;
                    this.charges[i].DiffImage.color = this.partColor;
                }
            }
            this.energyBarTooltip.TipText = $"{currentEnergy} / {energyInCharge * this.charges.Count}";
        }

        [DebuggerHidden]
        private IEnumerator Show(int chargeIndex) => 
            new <Show>c__Iterator0 { 
                chargeIndex = chargeIndex,
                $this = this
            };

        public void ShowEnergyResult()
        {
            int chargeIndex = (int) ((this.currentEnergy - this.earnedEnergy) / this.energyInCharge);
            if (chargeIndex < this.charges.Count)
            {
                base.StartCoroutine(this.Show(chargeIndex));
            }
        }

        [CompilerGenerated]
        private sealed class <Show>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal int chargeIndex;
            internal int <i>__1;
            internal float <chargeProgress>__2;
            internal EnergyResultUI $this;
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
                        this.$current = new WaitForSeconds(this.$this.duration);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto TR_0001;

                    case 1:
                        this.<i>__1 = this.chargeIndex;
                        break;

                    case 2:
                        this.<i>__1++;
                        break;

                    default:
                        goto TR_0000;
                }
                if (this.<i>__1 < this.$this.charges.Count)
                {
                    this.<chargeProgress>__2 = Mathf.Clamp01(((float) (this.$this.currentEnergy - (this.<i>__1 * this.$this.energyInCharge))) / ((float) this.$this.energyInCharge));
                    this.$this.charges[this.<i>__1].Set(this.$this.previousProgress[this.<i>__1], this.<chargeProgress>__2);
                    this.$current = new WaitForSeconds(this.$this.duration);
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                }
                else
                {
                    this.$PC = -1;
                    goto TR_0000;
                }
                goto TR_0001;
            TR_0000:
                return false;
            TR_0001:
                return true;
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

