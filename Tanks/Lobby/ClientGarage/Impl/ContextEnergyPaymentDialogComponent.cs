namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class ContextEnergyPaymentDialogComponent : BehaviourComponent
    {
        private long price;
        private long energy;
        [SerializeField]
        private LocalizedField highlightmMessageLocalization;
        [SerializeField]
        private LocalizedField messageLocalization;
        [SerializeField]
        private LocalizedField priceLocalization;
        [SerializeField]
        private TextMeshProUGUI messageText;
        [SerializeField]
        private TextMeshProUGUI priceText;
        private List<Animator> animators;
        private bool _show;

        public void BuyEnergy()
        {
            this.Hide();
            if (base.GetComponent<EntityBehaviour>() != null)
            {
                Entity entity = base.GetComponent<EntityBehaviour>().Entity;
                base.NewEvent(new PressEnergyContextBuyButtonEvent(this.energy, this.price)).Attach(entity).Schedule();
            }
        }

        public void Hide()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            this.show = false;
            base.GetComponent<Animator>().SetBool("show", false);
            if (this.animators != null)
            {
                foreach (Animator animator in this.animators)
                {
                    animator.SetBool("Visible", true);
                }
            }
        }

        public void HighlightQuickBattles()
        {
            this.Hide();
            EntityBehaviour component = base.GetComponent<EntityBehaviour>();
            if (component != null)
            {
                string message = this.highlightmMessageLocalization.Value;
                HighlightQuickData data = new HighlightQuickData(component.Entity, this.tipPositionRect, message) {
                    ContinueOnClick = true
                };
                TutorialCanvas.Instance.AddAllowSelectable(this.highlightedObject.GetComponentInChildren<Button>());
                GameObject[] highlightedRects = new GameObject[] { this.highlightedObject };
                TutorialCanvas.Instance.Show(data, true, highlightedRects, null);
            }
        }

        protected virtual void OnEnable()
        {
            base.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            base.GetComponent<Animator>().SetBool("show", true);
            if (this.animators != null)
            {
                foreach (Animator animator in this.animators)
                {
                    animator.SetBool("Visible", false);
                }
            }
        }

        public void OnHide()
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

        public void Show(List<Animator> animators)
        {
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
            this.animators = animators;
            this.show = true;
            if (base.gameObject.activeInHierarchy)
            {
                this.OnEnable();
            }
            else
            {
                base.gameObject.SetActive(true);
            }
        }

        public void Show(long energy, long price)
        {
            this.energy = energy;
            this.price = price;
            this.messageText.text = string.Format(this.messageLocalization.Value, energy);
            this.priceText.text = string.Format(this.priceLocalization.Value, price);
            this.Show(null);
        }

        public GameObject highlightedObject { get; set; }

        public RectTransform tipPositionRect { get; set; }

        public bool show
        {
            get => 
                this._show;
            set => 
                this._show = value;
        }

        public class HighlightQuickData : TutorialData
        {
            public HighlightQuickData(Entity entity, RectTransform popupPositionRect, string message)
            {
                base.Type = TutorialType.Default;
                base.Message = message;
                base.TutorialStep = base.TutorialStep;
                base.PopupPositionRect = popupPositionRect;
                base.ShowDelay = 0f;
                base.ImageUid = string.Empty;
            }
        }
    }
}

