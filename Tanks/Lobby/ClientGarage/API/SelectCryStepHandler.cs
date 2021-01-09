namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class SelectCryStepHandler : TutorialStepHandler
    {
        [SerializeField]
        private RectTransform popupPositionRect;
        [SerializeField]
        private RectTransform highlightedRects;

        private void Complete()
        {
            this.StepComplete();
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            CheckBoughtItemEvent eventInstance = new CheckBoughtItemEvent(data.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId);
            TutorialStepHandler.EngineService.Engine.ScheduleEvent(eventInstance, TutorialStepHandler.EngineService.EntityStub);
            if (eventInstance.TutorialItemAlreadyBought)
            {
                TutorialStepHandler.EngineService.Engine.ScheduleEvent<CompleteTutorialByEscEvent>(data.TutorialStep);
                this.Complete();
            }
            else
            {
                data.PopupPositionRect = this.popupPositionRect;
                data.ContinueOnClick = false;
                GameObject[] highlightedRects = new GameObject[] { this.highlightedRects.gameObject };
                TutorialCanvas.Instance.Show(data, true, highlightedRects, null);
                base.gameObject.SetActive(true);
                base.Invoke("Complete", 1f);
            }
        }
    }
}

