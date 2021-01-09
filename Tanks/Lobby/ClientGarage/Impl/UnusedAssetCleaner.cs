namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UnusedAssetCleaner : MonoBehaviour
    {
        private AsyncOperation _cleanOperation;

        [DebuggerHidden]
        private IEnumerator CleanCoroutine() => 
            new <CleanCoroutine>c__Iterator0 { $this = this };

        private void OnEnable()
        {
            this._cleanOperation = Resources.UnloadUnusedAssets();
            base.StartCoroutine(this.CleanCoroutine());
            GC.Collect();
        }

        [CompilerGenerated]
        private sealed class <CleanCoroutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal UnusedAssetCleaner $this;
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
                        if (!this.$this._cleanOperation.isDone)
                        {
                            Console.WriteLine("Start cleaning in " + DateTime.Now);
                            Console.WriteLine("UsedMemory: " + Process.GetCurrentProcess().WorkingSet64);
                        }
                        this.$current = this.$this._cleanOperation;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        if (this.$this._cleanOperation.isDone)
                        {
                            Console.WriteLine("Stop cleaning in " + DateTime.Now);
                            Console.WriteLine("UsedMemory: " + Process.GetCurrentProcess().WorkingSet64);
                        }
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

