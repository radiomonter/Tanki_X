namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class AnimatedDiffProgress : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField]
        private UIRectClipper fill;
        [SerializeField]
        private UIRectClipper background;
        [SerializeField]
        private UIRectClipper diff;
        [SerializeField]
        private float duration = 0.15f;
        private float normalizedValue;
        private float newValue;
        private Coroutine coroutine;

        [DebuggerHidden]
        private IEnumerator AnimateTo(float startValue, float targetValue, float startNewValue, float targetNewValue) => 
            new <AnimateTo>c__Iterator0 { 
                startValue = startValue,
                startNewValue = startNewValue,
                targetValue = targetValue,
                targetNewValue = targetNewValue,
                $this = this
            };

        private void Awake()
        {
            this.fill.FromX = 0f;
            this.fill.ToX = 0f;
            this.diff.FromX = 0f;
            this.diff.ToX = 0f;
            if (this.background != null)
            {
                this.background.FromX = 0f;
                this.background.ToX = 1f;
            }
        }

        private void OnEnable()
        {
            base.StartCoroutine(this.AnimateTo(0f, this.normalizedValue, this.normalizedValue, this.newValue));
        }

        public void Set(float value, float newValue)
        {
            if (this.coroutine != null)
            {
                base.StopCoroutine(this.coroutine);
                this.coroutine = null;
            }
            if (base.gameObject.activeInHierarchy)
            {
                this.coroutine = base.StartCoroutine(this.AnimateTo(this.normalizedValue, value, value, newValue));
            }
            this.normalizedValue = value;
            this.newValue = newValue;
        }

        public Image FillImage =>
            this.fill.GetComponent<Image>();

        public Image DiffImage =>
            this.diff.GetComponent<Image>();

        [CompilerGenerated]
        private sealed class <AnimateTo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal float <time>__0;
            internal float startValue;
            internal float <value>__0;
            internal float startNewValue;
            internal float targetValue;
            internal float targetNewValue;
            internal AnimatedDiffProgress $this;
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
                        this.<time>__0 = 0f;
                        this.<value>__0 = this.startValue;
                        this.$this.diff.ToX = this.startNewValue;
                        this.$this.diff.FromX = this.<value>__0;
                        break;

                    case 1:
                        this.<time>__0 += Time.deltaTime;
                        break;

                    default:
                        goto TR_0000;
                }
                if (this.<time>__0 < this.$this.duration)
                {
                    this.<value>__0 = this.startValue + ((this.targetValue - this.startValue) * this.$this.curve.Evaluate(Mathf.Clamp01(this.<time>__0 / this.$this.duration)));
                    this.$this.fill.ToX = this.<value>__0;
                    this.$this.diff.FromX = this.<value>__0;
                    this.$this.diff.ToX = this.startNewValue + ((this.targetNewValue - this.startNewValue) * this.$this.curve.Evaluate(Mathf.Clamp01(this.<time>__0 / this.$this.duration)));
                    if (this.$this.background != null)
                    {
                        this.$this.background.FromX = this.$this.diff.ToX;
                    }
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.fill.ToX = this.targetValue;
                this.$this.diff.FromX = this.targetValue;
                this.$this.diff.ToX = this.targetNewValue;
                if (this.$this.background != null)
                {
                    this.$this.background.FromX = this.targetNewValue;
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

