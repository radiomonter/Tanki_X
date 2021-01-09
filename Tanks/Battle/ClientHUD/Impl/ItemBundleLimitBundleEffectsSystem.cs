namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ItemBundleLimitBundleEffectsSystem : ECSSystem
    {
        [OnEventFire]
        public void Empty(NodeAddedEvent e, SingleNode<InventoryLimitBundleEffectsComponent> node)
        {
        }
    }
}

