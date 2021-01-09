namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientNavigation.Impl;
    using UnityEngine;

    public class ReConnectBehaviour : MonoBehaviour
    {
        [DebuggerHidden]
        private IEnumerator LoadState() => 
            new <LoadState>c__Iterator0 { $this = this };

        public void Start()
        {
            base.StartCoroutine(this.LoadState());
        }

        public int ReConnectTime { get; set; }

        [CompilerGenerated]
        private sealed class <LoadState>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal ReConnectBehaviour $this;
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
                        this.$current = new WaitForSeconds((float) this.$this.ReConnectTime);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        SceneSwitcher.CleanAndRestart();
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

