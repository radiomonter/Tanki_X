namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class ExitGameDialog : ConfirmWindowComponent
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            DailyBonusScreenSoundsRoot component = UISoundEffectController.UITransformRoot.GetComponent<DailyBonusScreenSoundsRoot>();
            if (component)
            {
                component.dailyBonusSoundsBehaviour.PlayClick();
            }
        }
    }
}

