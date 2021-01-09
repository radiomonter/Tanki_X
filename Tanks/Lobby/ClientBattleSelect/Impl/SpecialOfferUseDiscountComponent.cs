namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class SpecialOfferUseDiscountComponent : BehaviourComponent
    {
        public void OnClick()
        {
            ShowXCrystalsDialogEvent eventInstance = new ShowXCrystalsDialogEvent {
                ShowTitle = true
            };
            base.ScheduleEvent(eventInstance, base.GetComponent<EntityBehaviour>().Entity);
        }
    }
}

