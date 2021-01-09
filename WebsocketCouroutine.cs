using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WebsocketCouroutine : MonoBehaviour
{
    [DebuggerHidden]
    public IEnumerator OneShot(IEnumerator coroutine) => 
        new <OneShot>c__Iterator0 { 
            coroutine = coroutine,
            $this = this
        };

    public static void StartOneShotCoroutine(IEnumerator coroutine)
    {
        WebsocketCouroutine couroutine = new GameObject("WebsocketCouroutine").AddComponent<WebsocketCouroutine>();
        couroutine.StartCoroutine(couroutine.OneShot(coroutine));
    }

    [CompilerGenerated]
    private sealed class <OneShot>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal IEnumerator coroutine;
        internal WebsocketCouroutine $this;
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
                    this.$current = this.$this.StartCoroutine(this.coroutine);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    Object.Destroy(this.$this.gameObject);
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

