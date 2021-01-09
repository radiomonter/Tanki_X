namespace Tanks.Lobby.ClientNavigation.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DisableFromAnimation : MonoBehaviour
    {
        public void DisableGameObjectFromAnimation()
        {
            base.StartCoroutine(this.DisableGameObjectOnEndOfFrame());
        }

        [DebuggerHidden]
        private IEnumerator DisableGameObjectOnEndOfFrame() => 
            new <DisableGameObjectOnEndOfFrame>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <DisableGameObjectOnEndOfFrame>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal DisableFromAnimation $this;
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
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.gameObject.SetActive(false);
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

