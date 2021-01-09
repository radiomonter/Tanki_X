namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;

    public class ElementLockSystem : ECSSystem
    {
        [OnEventFire]
        public void LockElement(LockElementEvent e, Node node, [JoinAll, Combine] SingleNode<LockedElementComponent> lockedElement)
        {
            lockedElement.component.canvasGroup.blocksRaycasts = false;
        }

        [OnEventFire]
        public void UnlockElement(UnlockElementEvent e, Node node, [JoinAll, Combine] SingleNode<LockedElementComponent> lockedElement)
        {
            lockedElement.component.canvasGroup.blocksRaycasts = true;
        }
    }
}

