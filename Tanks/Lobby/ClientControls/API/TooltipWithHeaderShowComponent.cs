namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class TooltipWithHeaderShowComponent : TooltipShowBehaviour
    {
        public LocalizedField header;
        protected string headerTipText = string.Empty;

        protected override void Awake()
        {
            base.Awake();
            if (string.IsNullOrEmpty(this.headerTipText) && !string.IsNullOrEmpty(this.header.Value))
            {
                this.HeaderTipText = this.header.Value;
            }
        }

        public override void ShowTooltip(Vector3 mousePosition)
        {
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            TooltipShowBehaviour.EngineService.Engine.ScheduleEvent(eventInstance, TooltipShowBehaviour.EngineService.EntityStub);
            if (!eventInstance.TutorialIsActive)
            {
                base.tooltipShowed = true;
                string[] data = new string[] { this.HeaderTipText, base.TipText };
                TooltipController.Instance.ShowTooltip(mousePosition, data, !base.customContentPrefab ? null : base.customPrefab, true);
            }
        }

        public virtual string HeaderTipText
        {
            get => 
                this.headerTipText;
            set => 
                this.headerTipText = value;
        }
    }
}

