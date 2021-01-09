namespace Tanks.Lobby.ClientControls.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AnimationTriggerDelayBehaviour : MonoBehaviour
    {
        public float dealy;
        public Animator animator;
        public string trigger;

        [DebuggerHidden]
        private IEnumerator ExecuteAfterTime(float time) => 
            new <ExecuteAfterTime>c__Iterator0 { 
                time = time,
                $this = this
            };

        private void Start()
        {
            base.StartCoroutine(this.ExecuteAfterTime(this.dealy));
        }

        [CompilerGenerated]
        private sealed class <ExecuteAfterTime>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal float time;
            internal AnimationTriggerDelayBehaviour $this;
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
                        this.$current = new WaitForSeconds(this.time);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.animator.SetTrigger(this.$this.trigger);
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

