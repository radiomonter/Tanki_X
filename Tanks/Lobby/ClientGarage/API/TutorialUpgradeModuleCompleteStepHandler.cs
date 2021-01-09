namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TutorialUpgradeModuleCompleteStepHandler : TutorialStepHandler
    {
        private void Complete()
        {
            ShopTabManager.shopTabIndex = 1;
            MainScreenComponent.Instance.HideQuestsIfVisible();
            MainScreenComponent.Instance.ShowShopIfNotVisible();
            this.StepComplete();
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            OpenTutorialContainerDialogEvent eventInstance = new OpenTutorialContainerDialogEvent {
                StepId = data.TutorialStep.GetComponent<TutorialStepDataComponent>().StepId,
                ItemId = data.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId
            };
            TutorialStepHandler.EngineService.Engine.ScheduleEvent(eventInstance, TutorialStepHandler.EngineService.EntityStub);
            eventInstance.dialog.Message = data.Message;
            TutorialContainerDialog dialog = eventInstance.dialog;
            dialog.dialogClosed += new Action(this.Complete);
        }
    }
}

