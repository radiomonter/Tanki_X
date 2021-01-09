namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class BaseSelectScreenComponent : LocalizedScreenComponent, PaymentScreen
    {
        private IUIList list;

        protected override void Awake()
        {
            base.Awake();
            this.list = base.GetComponentInChildren<IUIList>();
        }

        public IUIList List =>
            this.list;
    }
}

