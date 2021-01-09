namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TutorialWeaponControlsHideTriggerComponent : TutorialHideTriggerComponent
    {
        protected void Update()
        {
            if (!base.triggered && InputManager.CheckAction(ShotActions.SHOT))
            {
                this.Triggered();
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

