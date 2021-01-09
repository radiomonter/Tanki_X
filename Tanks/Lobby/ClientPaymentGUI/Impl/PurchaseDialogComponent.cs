namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class PurchaseDialogComponent : PurchaseItemComponent
    {
        public void Clear()
        {
            base.shopDialogs = null;
            base.methods.Clear();
        }

        public void ShowDialog(Entity goodsEntity)
        {
            base.OnPackClick(goodsEntity, false);
        }
    }
}

