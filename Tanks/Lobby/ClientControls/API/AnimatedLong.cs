namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientLocale.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;

    public class AnimatedLong : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField]
        private float duration = 0.15f;
        [SerializeField]
        private TextMeshProUGUI numberText;
        [SerializeField]
        private string format = "{0:#}";
        private long value = -1L;
        private Coroutine coroutine;
        private Animator animator;
        private bool immediatePending;

        [DebuggerHidden]
        private IEnumerator AnimateTo(long startValue, long targetValue) => 
            new <AnimateTo>c__Iterator0 { 
                targetValue = targetValue,
                startValue = startValue,
                $this = this
            };

        private void OnEnable()
        {
            this.animator = base.GetComponent<Animator>();
            if (!this.immediatePending)
            {
                this.coroutine = base.StartCoroutine(this.AnimateTo(0L, this.value));
            }
            this.immediatePending = false;
        }

        public void SetFormat(string newFormat)
        {
            this.format = newFormat;
        }

        public void SetImmediate(long value)
        {
            if (this.coroutine != null)
            {
                base.StopCoroutine(this.coroutine);
                this.coroutine = null;
            }
            this.StopAnimation();
            this.value = value;
            this.SetText(value);
            this.immediatePending = !base.gameObject.activeInHierarchy;
        }

        private void SetText(long val)
        {
            object[] args = new object[] { val };
            this.numberText.text = string.Format(LocaleUtils.GetCulture(), this.format, args);
        }

        private void StartAnimation()
        {
            if (this.animator != null)
            {
                this.animator.SetBool("animated", true);
            }
        }

        private void StopAnimation()
        {
            if (this.animator != null)
            {
                this.animator.SetBool("animated", false);
            }
        }

        public long Value
        {
            get => 
                this.value;
            set
            {
                if (this.value != value)
                {
                    if (this.coroutine != null)
                    {
                        base.StopCoroutine(this.coroutine);
                        this.coroutine = null;
                    }
                    this.StopAnimation();
                    if (base.gameObject.activeInHierarchy)
                    {
                        this.coroutine = base.StartCoroutine(this.AnimateTo(this.value, value));
                    }
                    this.value = value;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <AnimateTo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal long targetValue;
            internal long startValue;
            internal float <time>__0;
            internal long <val>__0;
            internal AnimatedLong $this;
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
                        if (this.targetValue > this.startValue)
                        {
                            this.$this.StartAnimation();
                        }
                        this.<time>__0 = 0f;
                        this.<val>__0 = this.startValue;
                        break;

                    case 1:
                        this.<time>__0 += Time.deltaTime;
                        break;

                    default:
                        goto TR_0000;
                }
                if (!Mathf.Approximately((float) this.<val>__0, (float) this.targetValue))
                {
                    this.<val>__0 = this.startValue + ((long) ((this.targetValue - this.startValue) * this.$this.curve.Evaluate(Mathf.Clamp01(this.<time>__0 / this.$this.duration))));
                    this.$this.SetText(this.<val>__0);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.StopAnimation();
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

