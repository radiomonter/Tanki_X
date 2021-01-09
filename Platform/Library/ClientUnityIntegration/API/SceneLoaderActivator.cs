namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Library.ClientUnityIntegration;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneLoaderActivator : UnityAwareActivator<ManuallyCompleting>
    {
        public List<string> environmentSceneNames;
        public float progress;

        protected override void Activate()
        {
            base.StartCoroutine(this.LoadScenes());
        }

        [DebuggerHidden]
        private IEnumerator LoadScenes() => 
            new <LoadScenes>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <LoadScenes>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal int <i>__1;
            internal SceneLoaderActivator $this;
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
                        this.<i>__1 = 0;
                        break;

                    case 1:
                        this.$this.progress += 1f / ((float) this.$this.environmentSceneNames.Count);
                        this.<i>__1++;
                        break;

                    case 2:
                        this.$this.Complete();
                        this.$PC = -1;
                        goto TR_0000;

                    default:
                        goto TR_0000;
                }
                if (this.<i>__1 < this.$this.environmentSceneNames.Count)
                {
                    this.$current = SceneManager.LoadSceneAsync(this.$this.environmentSceneNames[this.<i>__1], LoadSceneMode.Additive);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                }
                else
                {
                    this.$this.progress = 1f;
                    this.$current = new WaitForEndOfFrame();
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
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

