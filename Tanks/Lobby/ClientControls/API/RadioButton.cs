namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class RadioButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject inactiveHighlight;

        public virtual void Activate()
        {
            this.Selectable.interactable = false;
            this.inactiveHighlight.SetActive(true);
            IEnumerator enumerator = base.transform.parent.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (current != base.transform)
                    {
                        RadioButton component = current.GetComponent<RadioButton>();
                        if (component != null)
                        {
                            component.Deactivate();
                        }
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public virtual void Deactivate()
        {
            this.Selectable.interactable = true;
            this.inactiveHighlight.SetActive(false);
            this.Selectable.OnPointerExit(null);
        }

        public UnityEngine.UI.Selectable Selectable =>
            base.GetComponent<UnityEngine.UI.Selectable>();
    }
}

