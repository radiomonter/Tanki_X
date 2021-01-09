namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class HighlightModuleAndCryStepHandler : TutorialStepHandler
    {
        [SerializeField]
        private RectTransform popupPosition;
        [SerializeField]
        private RectTransform crysRect;

        private void Complete()
        {
            this.StepComplete();
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            TutorialCanvas.Instance.AddAdditionalMaskRect(this.crysRect.gameObject);
            data.PopupPositionRect = this.popupPosition;
            data.ContinueOnClick = false;
            TutorialCanvas.Instance.Show(data, true, null, null);
            base.Invoke("Complete", 1f);
        }
    }
}

