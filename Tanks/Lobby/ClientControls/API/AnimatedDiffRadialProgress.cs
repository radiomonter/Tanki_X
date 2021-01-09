namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class AnimatedDiffRadialProgress : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField]
        private Image fill;
        [SerializeField]
        private Image background;
        [SerializeField]
        private Image diff;
        [SerializeField]
        private float duration = 0.15f;
        [SerializeField]
        private float normalizedValue;
        [SerializeField]
        private float newValue;
        private Coroutine coroutine;

        [DebuggerHidden]
        private IEnumerator AnimateTo(float startValue, float targetValue, float startNewValue, float targetNewValue) => 
            new <AnimateTo>c__Iterator0 { 
                startValue = startValue,
                targetValue = targetValue,
                startNewValue = startNewValue,
                targetNewValue = targetNewValue,
                $this = this
            };

        private void Awake()
        {
            this.fill.fillAmount = 0f;
            this.diff.fillAmount = 0f;
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

        private void Update()
        {
            if (!Application.isPlaying)
            {
                this.diff.fillAmount = this.normalizedValue;
                this.fill.fillAmount = this.normalizedValue + (this.newValue - this.normalizedValue);
            }
        }

        [CompilerGenerated]
        private sealed class <AnimateTo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal float <time>__0;
            internal float startValue;
            internal float <value>__0;
            internal float targetValue;
            internal float startNewValue;
            internal float targetNewValue;
            internal AnimatedDiffRadialProgress $this;
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
                    this.$this.diff.fillAmount = this.<value>__0;
                    this.$this.fill.fillAmount = this.startNewValue + ((this.targetNewValue - this.startNewValue) * this.$this.curve.Evaluate(Mathf.Clamp01(this.<time>__0 / this.$this.duration)));
                    this.$current = null;
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

