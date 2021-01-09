namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Linq;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;

    public class SpecialOfferUiSystem : ECSSystem
    {
        [OnEventFire]
        public void ClickOpenContainerButton(ButtonClickEvent e, SingleNode<SpecialOfferOpenContainerButton> button, [JoinAll] SingleNode<BattleResultsAwardsScreenComponent> screen)
        {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(button.component.containerId);
            BattleResultAwardsScreenSystem.GamePlayChestItemNode node = base.Select<BattleResultAwardsScreenSystem.GamePlayChestItemNode>(entity, typeof(MarketItemGroupComponent)).FirstOrDefault<BattleResultAwardsScreenSystem.GamePlayChestItemNode>();
            if ((node != null) && (node.userItemCounter.Count != 0L))
            {
                OpenContainerEvent eventInstance = new OpenContainerEvent {
                    Amount = button.component.quantity
                };
                base.ScheduleEvent(eventInstance, node);
                button.component.onOpen();
            }
        }
    }
}

