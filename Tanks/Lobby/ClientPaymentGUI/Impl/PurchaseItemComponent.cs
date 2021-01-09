namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class PurchaseItemComponent : BehaviourComponent
    {
        [NonSerialized]
        public ShopDialogs shopDialogs;
        private bool steamComponentIsPresent;
        protected readonly HashSet<Entity> methods = new HashSet<Entity>();
        [SerializeField]
        private LocalizedField specialOfferDesc;
        private string steamOfflineDescUid = "b4eafa32-5752-4cd8-b1ae-c9aa9a702bac";

        public void AddMethod(Entity entity)
        {
        }

        private void CloseDialogs()
        {
            if ((this.shopDialogs != null) && this.shopDialogs.gameObject.activeInHierarchy)
            {
                this.shopDialogs.CloseAll();
            }
        }

        public void HandleError()
        {
            if (this.shopDialogs != null)
            {
                this.shopDialogs.ShowError();
            }
        }

        public void HandleGoToLink()
        {
            this.CloseDialogs();
        }

        public void HandleQiwiError()
        {
            if ((this.shopDialogs != null) && this.shopDialogs.gameObject.activeInHierarchy)
            {
                this.shopDialogs.ShowQiwiError();
            }
        }

        public void HandleSuccess()
        {
            this.CloseDialogs();
        }

        public void HandleSuccessMobile(string transactionId)
        {
            if ((this.shopDialogs != null) && this.shopDialogs.gameObject.activeInHierarchy)
            {
                this.shopDialogs.ShowCheckout(transactionId);
            }
        }

        protected void OnPackClick(Entity entity, bool xCry = false)
        {
            this.shopDialogs.Show(entity, this.methods, xCry, this.specialOfferDesc.Value);
        }

        public void ShowPurchaseDialog(ShopDialogs shopDialogs, Entity entity, bool xCry = false)
        {
            this.shopDialogs = shopDialogs;
            this.OnPackClick(entity, xCry);
        }

        public bool SteamComponentIsPresent
        {
            get => 
                this.steamComponentIsPresent;
            set => 
                this.steamComponentIsPresent = value;
        }
    }
}

