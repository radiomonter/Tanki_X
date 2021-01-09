namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class DailyBonusMainScreenButtonComponent : BehaviourComponent
    {
        private const string IS_ACTIVE = "isActive";
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private LocalizedField enabledTip;
        [SerializeField]
        private LocalizedField disabledTipTip;
        [SerializeField]
        private TooltipShowBehaviour tooltipShow;
        private bool? isActiveState;
        private bool? interactable;

        public void ResetState()
        {
            bool? nullable = null;
            this.interactable = nullable;
            nullable = null;
            this.isActiveState = nullable;
        }

        public bool IsActiveState
        {
            set
            {
                if (this.isActiveState == null)
                {
                    this.isActiveState = new bool?(value);
                    this.animator.SetBool("isActive", value);
                }
                else if (this.isActiveState.Value ^ value)
                {
                    this.isActiveState = new bool?(value);
                    this.animator.SetBool("isActive", value);
                }
            }
        }

        public bool Interactable
        {
            set
            {
                this.canvasGroup.interactable = value;
                this.canvasGroup.alpha = !value ? 0.5f : 1f;
                if (this.interactable == null)
                {
                    this.tooltipShow.localizedTip = !value ? this.disabledTipTip : this.enabledTip;
                    this.tooltipShow.UpdateTipText();
                    this.interactable = new bool?(value);
                }
                else if (this.interactable.Value ^ value)
                {
                    this.tooltipShow.localizedTip = !value ? this.disabledTipTip : this.enabledTip;
                    this.tooltipShow.UpdateTipText();
                    this.interactable = new bool?(value);
                }
            }
        }
    }
}

