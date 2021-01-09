namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;

    public class TMPCaretFix : MonoBehaviour
    {
        [DebuggerHidden]
        private IEnumerator Delay() => 
            new <Delay>c__Iterator0 { $this = this };

        private void OnEnable()
        {
            base.StartCoroutine(this.Delay());
        }

        [CompilerGenerated]
        private sealed class <Delay>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal TMP_SelectionCaret <caret>__0;
            internal TMPCaretFix $this;
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
                        this.<caret>__0 = this.$this.GetComponentInChildren<TMP_SelectionCaret>();
                        if (this.<caret>__0 != null)
                        {
                            this.<caret>__0.rectTransform.anchoredPosition = new Vector2(-5f, 0f);
                            this.<caret>__0.rectTransform.sizeDelta = new Vector2(-10f, 0f);
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

