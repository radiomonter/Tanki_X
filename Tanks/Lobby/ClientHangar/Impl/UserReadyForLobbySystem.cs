namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;

    public class UserReadyForLobbySystem : ECSSystem
    {
        [OnEventFire]
        public void MarkUserAsReadyForLobby(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, UserNode selfUser)
        {
            selfUser.Entity.AddComponent<UserReadyForLobbyComponent>();
            GC.Collect();
        }

        [OnEventFire]
        public void UnmarkUserAsReadyForLobby(NodeRemoveEvent e, SingleNode<HangarInstanceComponent> hangar, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            selfUser.Entity.RemoveComponent<UserReadyForLobbyComponent>();
            GC.Collect();
        }

        [Not(typeof(UserReadyForLobbyComponent))]
        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
        }
    }
}

