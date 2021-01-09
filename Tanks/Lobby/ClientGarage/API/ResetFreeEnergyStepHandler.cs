namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class ResetFreeEnergyStepHandler : AddItemStepHandler
    {
        public override void RunStep(TutorialData data)
        {
            ShopTabManager.shopTabIndex = 0;
            base.RunStep(data);
        }
    }
}

