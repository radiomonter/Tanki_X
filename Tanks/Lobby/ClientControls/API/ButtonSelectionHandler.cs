namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [RequireComponent(typeof(Selectable))]
    public class ButtonSelectionHandler : MonoBehaviour, IDeselectHandler, IPointerEnterHandler, IEventSystemHandler
    {
        public void OnDeselect(BaseEventData eventData)
        {
            base.StartCoroutine(this.SelectAgain());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Selectable component = base.GetComponent<Selectable>();
            if (component.interactable)
            {
                component.Select();
            }
        }

        [DebuggerHidden]
        private IEnumerator SelectAgain() => 
            new <SelectAgain>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <SelectAgain>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal GameObject <newSelection>__0;
            internal ButtonSelectionHandler $this;
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
                        this.<newSelection>__0 = EventSystem.current.currentSelectedGameObject;
                        if ((this.<newSelection>__0 == null) || ((this.<newSelection>__0.GetComponent<ButtonSelectionHandler>() == null) && ((this.<newSelection>__0.GetComponent<InputField>() == null) && (this.<newSelection>__0.GetComponent<InputFieldComponent>() == null))))
                        {
                            this.$this.GetComponent<Selectable>().Select();
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

