namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class TankBuilderMaterialAlphaController : BehaviourComponent
    {
        private CanvasGroup[] canvasGroups;
        private Material material;

        private void Awake()
        {
            this.canvasGroups = base.GetComponentsInParent<CanvasGroup>();
            Image component = base.GetComponent<Image>();
            Material material = new Material(component.material);
            component.material = material;
            this.material = material;
        }

        private void OnEnable()
        {
            this.SetAlpha();
        }

        private void SetAlpha()
        {
            float num = 1f;
            foreach (CanvasGroup group in this.canvasGroups)
            {
                num *= group.alpha;
            }
            this.material.SetFloat("alpha", 1f - num);
        }

        private void Update()
        {
            this.SetAlpha();
        }
    }
}

