namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class GameplayChestResultUI : MonoBehaviour
    {
        [SerializeField]
        private AnimatedDiffRadialProgress progress;
        [SerializeField]
        protected TextMeshProUGUI progressValue;
        [SerializeField]
        private ImageSkin chestIcon;
        [SerializeField]
        private Button openChestButton;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private TooltipShowBehaviour progressTooltip;
        [SerializeField]
        private LocalizedField progressValueFormat;
        public long chestCount;
        [SerializeField]
        private long previousScores;
        [SerializeField]
        private long earnedScores;
        [SerializeField]
        private long limitScores;
        private float previousProgress;

        [DebuggerHidden]
        private IEnumerator AnimateProgress() => 
            new <AnimateProgress>c__Iterator0 { $this = this };

        public void OpenGameplayChest()
        {
            this.animator.SetTrigger("ChestRewardTaken");
            this.previousProgress = 0f;
            float num = Mathf.Clamp01(((float) (this.previousScores + this.earnedScores)) / ((float) this.limitScores));
            this.progress.Set(num, num);
            this.chestCount = 0L;
            this.ShowGameplayChestResult();
        }

        public void ResetProgress()
        {
            this.previousProgress = 0f;
            this.progress.Set(0f, 0f);
            this.chestCount -= 1L;
            this.ShowGameplayChestResult();
        }

        public void SetGameplayChestResult(string icon, long currentScores, long limitScores, long earnedScores)
        {
            this.earnedScores = earnedScores;
            this.limitScores = limitScores;
            this.progressValue.text = string.Format((string) this.progressValueFormat, earnedScores);
            this.chestIcon.SpriteUid = icon;
            long num = (currentScores - earnedScores) % limitScores;
            this.previousScores = (num < 0L) ? (limitScores + num) : num;
            this.previousProgress = Mathf.Clamp01(((float) this.previousScores) / ((float) limitScores));
            this.progress.Set(this.previousProgress, this.previousProgress);
            this.progressTooltip.gameObject.SetActive(false);
            this.openChestButton.gameObject.SetActive(false);
        }

        public void ShowGameplayChestResult()
        {
            if (((this.previousScores + this.earnedScores) >= this.limitScores) && (this.chestCount > 1L))
            {
                this.progress.Set(this.previousProgress, 1f);
                this.earnedScores -= this.limitScores - this.previousScores;
                this.previousScores = 0L;
                base.StartCoroutine(this.AnimateProgress());
            }
            if (((this.previousScores + this.earnedScores) >= this.limitScores) && (this.chestCount == 1L))
            {
                this.progress.Set(this.previousProgress, 1f);
                this.animator.SetTrigger("GotChest");
                this.earnedScores -= this.limitScores - this.previousScores;
                this.previousScores = 0L;
                base.StartCoroutine(this.AnimateProgress());
            }
            if (((this.previousScores + this.earnedScores) < this.limitScores) && (this.chestCount < 1L))
            {
                float newValue = Mathf.Clamp01(((float) (this.previousScores + this.earnedScores)) / ((float) this.limitScores));
                this.progress.Set(this.previousProgress, newValue);
                this.progressTooltip.gameObject.SetActive(true);
                this.progressTooltip.TipText = $"{this.previousScores + this.earnedScores} / {this.limitScores}";
            }
        }

        [CompilerGenerated]
        private sealed class <AnimateProgress>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal GameplayChestResultUI $this;
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
                        this.$current = new WaitForSeconds(0.3f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.ResetProgress();
                        this.$PC = -1;
                        break;

                    default:
                        break;
                }
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

