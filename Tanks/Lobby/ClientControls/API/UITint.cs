namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(Graphic))]
    public class UITint : MonoBehaviour
    {
        private UITintController controller;
        private Graphic graphic;

        private void Awake()
        {
            this.graphic = base.GetComponent<Graphic>();
        }

        private void ClearController()
        {
            if (this.controller != null)
            {
                this.controller.RemoveElement(this);
            }
            this.controller = null;
        }

        private void InitController()
        {
            UITintController componentInParent = base.GetComponentInParent<UITintController>();
            if (componentInParent != null)
            {
                componentInParent.AddElement(this);
            }
            this.controller = componentInParent;
        }

        private void OnDisable()
        {
            this.ClearController();
        }

        private void OnEnable()
        {
            this.ClearController();
            this.InitController();
        }

        private void OnTransformParentChanged()
        {
            this.ClearController();
            this.InitController();
        }

        public virtual void SetTint(Color tint)
        {
            this.graphic.color = tint;
        }
    }
}

