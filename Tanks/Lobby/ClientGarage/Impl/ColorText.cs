namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(Text))]
    public class ColorText : MonoBehaviour, IColorButtonElement
    {
        public bool noApplyMaterial;
        private ColorButtonController controller;
        public Text text;

        private void Awake()
        {
            this.text = base.GetComponent<Text>();
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
            ColorButtonController componentInParent = base.GetComponentInParent<ColorButtonController>();
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

        public virtual void SetColor(ColorData color)
        {
            this.text.color = color.color;
            if (!this.noApplyMaterial)
            {
                this.text.material = color.material;
            }
        }
    }
}

