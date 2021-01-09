namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientPayment.API;
    using UnityEngine;

    public class GiftsPromoComponent : BehaviourComponent, ContentWithOrder
    {
        public int order = 100;

        public void SetParent(Transform parent)
        {
            base.transform.SetParent(parent, false);
        }

        public void Show()
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent(new GoToXCryShopScreen(), new EntityStub());
        }

        public int Order =>
            this.order;

        public bool CanFillBigRow =>
            true;

        public bool CanFillSmallRow =>
            false;
    }
}

