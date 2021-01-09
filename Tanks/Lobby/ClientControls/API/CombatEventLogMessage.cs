namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Animator))]
    public class CombatEventLogMessage : BaseCombatLogMessageElement
    {
        [SerializeField]
        private float messageTimeout;
        [SerializeField]
        private UnityEngine.UI.LayoutElement layoutElement;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        protected RectTransform placeholder;
        protected RectTransform rightElement;
        private bool deleteRequested;

        public virtual void Attach(RectTransform child, bool toRight)
        {
            child.SetParent(this.placeholder, false);
            if (toRight)
            {
                if (this.rightElement != null)
                {
                    this.rightElement.SetParent(child, false);
                    (this.rightElement.GetComponent<UnityEngine.UI.LayoutElement>() ?? this.rightElement.gameObject.AddComponent<UnityEngine.UI.LayoutElement>()).ignoreLayout = true;
                }
                this.rightElement = child;
            }
        }

        private void Delete()
        {
            base.SendMessageUpwards("OnDeleteMessage", this);
        }

        public void RequestDelete()
        {
            if (!this.deleteRequested && this.animator)
            {
                this.deleteRequested = true;
                this.animator.SetTrigger("Hide");
            }
        }

        private void SendScroll()
        {
            base.SendMessageUpwards("OnScrollLog", this.layoutElement.preferredHeight);
        }

        public void ShowMessage()
        {
            this.animator.SetTrigger("Show");
        }

        public UnityEngine.UI.LayoutElement LayoutElement =>
            this.layoutElement;
    }
}

