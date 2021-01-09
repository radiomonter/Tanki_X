namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator)), RequireComponent(typeof(CanvasGroup))]
    public class Tab : MonoBehaviour
    {
        [SerializeField]
        protected RadioButton button;
        private bool _show;

        public virtual void Hide()
        {
            this.show = false;
            if (base.gameObject.activeInHierarchy)
            {
                base.GetComponent<Animator>().SetBool("show", false);
            }
            else
            {
                base.gameObject.SetActive(false);
            }
            base.SendMessage("OnHide", SendMessageOptions.DontRequireReceiver);
        }

        protected virtual void OnEnable()
        {
            base.GetComponent<CanvasGroup>().alpha = 0f;
            base.GetComponent<Animator>().SetBool("show", true);
        }

        public virtual void OnHid()
        {
            if (this.show)
            {
                this.OnEnable();
            }
            else
            {
                base.gameObject.SetActive(false);
            }
        }

        public virtual void Show()
        {
            this.show = true;
            this.button.Activate();
            if (base.gameObject.activeInHierarchy)
            {
                this.OnEnable();
            }
            else
            {
                base.gameObject.SetActive(true);
            }
        }

        public bool show
        {
            get => 
                this._show;
            set => 
                this._show = value;
        }
    }
}

