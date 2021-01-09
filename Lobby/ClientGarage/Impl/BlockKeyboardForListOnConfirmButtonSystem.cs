namespace Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class BlockKeyboardForListOnConfirmButtonSystem : ECSSystem
    {
        [OnEventFire]
        public void Block(ConfirmButtonClickEvent e, Node node, [JoinAll, Combine] SingleNode<SimpleHorizontalListComponent> list)
        {
            list.component.IsKeyboardNavigationAllowed = false;
        }

        [OnEventFire]
        public void Unblock(ConfirmButtonClickNoEvent e, Node node, [JoinAll, Combine] SingleNode<SimpleHorizontalListComponent> list)
        {
            list.component.IsKeyboardNavigationAllowed = true;
        }

        [OnEventFire]
        public void Unblock(ConfirmButtonClickYesEvent e, Node node, [JoinAll, Combine] SingleNode<SimpleHorizontalListComponent> list)
        {
            list.component.IsKeyboardNavigationAllowed = true;
        }
    }
}

