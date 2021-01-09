namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class KillAssistEventLogMessage : CombatEventLogMessage
    {
        public override void Attach(RectTransform child, bool toRight)
        {
            child.SetParent(base.placeholder, false);
            if (toRight)
            {
                if (base.rightElement != null)
                {
                    base.rightElement.SetParent(child, false);
                    (base.rightElement.GetComponent<LayoutElement>() ?? base.rightElement.gameObject.AddComponent<LayoutElement>()).ignoreLayout = true;
                    base.StartCoroutine(this.DisplaceParent(base.rightElement));
                }
                base.rightElement = child;
            }
        }

        [DebuggerHidden]
        private IEnumerator DisplaceParent(RectTransform nick) => 
            new <DisplaceParent>c__Iterator0 { 
                nick = nick,
                $this = this
            };

        [CompilerGenerated]
        private sealed class <DisplaceParent>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Vector2 <position>__0;
            internal RectTransform nick;
            internal KillAssistEventLogMessage $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public unsafe bool MoveNext()
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
                    {
                        this.<position>__0 = this.$this.rightElement.anchoredPosition;
                        Vector2* vectorPtr1 = &this.<position>__0;
                        vectorPtr1->x += this.nick.rect.width;
                        this.$this.rightElement.anchoredPosition = this.<position>__0;
                        this.$PC = -1;
                        break;
                    }
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

