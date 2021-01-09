namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(Image))]
    public class ColorButton : MonoBehaviour, IColorButtonElement
    {
        public bool noApplyMaterial;
        public bool hardlight;
        private ColorButtonController controller;
        private Image image;

        private void Awake()
        {
            this.InitElement();
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

        private void InitElement()
        {
            this.image = base.GetComponent<Image>();
        }

        private void OnDisable()
        {
            this.ClearController();
        }

        private void OnEnable()
        {
            this.InitElement();
            this.ClearController();
            this.InitController();
        }

        private void OnTransformParentChanged()
        {
            this.ClearController();
            this.InitController();
        }

        public void SetColor(ColorData colorData)
        {
            this.image.color = !this.hardlight ? colorData.color : colorData.hardlightColor;
            if (!this.noApplyMaterial)
            {
                this.image.material = colorData.material;
            }
        }
    }
}

