namespace Tanks.Lobby.ClientControls.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class MoneyIndicatorVisualTest : MonoBehaviour
    {
        public UserMoneyIndicatorComponent userMoneyIndicator;

        private void Start()
        {
            base.StartCoroutine(this.Test());
        }

        [DebuggerHidden]
        private IEnumerator Test() => 
            new <Test>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <Test>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal MoneyIndicatorVisualTest $this;
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
                        break;

                    case 1:
                        this.$this.userMoneyIndicator.SetMoneyAnimated((long) Random.Range(0, 0x5f5e0ff));
                        break;

                    default:
                        return false;
                }
                this.$current = new WaitForSeconds((float) Random.Range(2, 10));
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
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

