namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class PurchasedItemListComponent : Component
    {
        private List<long> purchasedItems = new List<long>();

        public void AddPurchasedItem(long marketItemId)
        {
            this.purchasedItems.Add(marketItemId);
        }

        public bool Contains(long marketItemId) => 
            this.purchasedItems.Contains(marketItemId);
    }
}

