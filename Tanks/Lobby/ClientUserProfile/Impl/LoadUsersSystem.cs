namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientUserProfile.API;

    public class LoadUsersSystem : ECSSystem
    {
        [OnEventFire]
        public void AddStorage(NodeAddedEvent e, SingleNode<SelfUserComponent> selfUser)
        {
            selfUser.Entity.AddComponent<SharedUsersStorageComponent>();
        }

        [OnEventFire]
        public void LoadUser(NodeAddedEvent e, SingleNode<LoadUserComponent> loadUser, [JoinAll] SingleNode<SharedUsersStorageComponent> storage, [JoinByUser] SingleNode<ClientSessionComponent> session)
        {
            if (storage.component.UserId2EntityIdMap.ContainsKey(loadUser.component.UserId))
            {
                base.ScheduleEvent<UsersLoadedInternalEvent>(loadUser);
            }
            else
            {
                long[] usersIds = new long[] { loadUser.component.UserId };
                LoadUsersEvent eventInstance = new LoadUsersEvent(loadUser.Entity.Id, usersIds);
                base.ScheduleEvent(eventInstance, session);
            }
            storage.component.UserId2EntityIdMap.Add(loadUser.component.UserId, loadUser.Entity.Id);
        }

        [OnEventFire]
        public void LoadUsers(NodeAddedEvent e, SingleNode<LoadUsersComponent> loadUsers, [JoinAll] SingleNode<SharedUsersStorageComponent> storage, [JoinByUser] SingleNode<ClientSessionComponent> session)
        {
            HashSet<long> usersId = new HashSet<long>();
            foreach (long num in loadUsers.component.UserIds)
            {
                if (!storage.component.UserId2EntityIdMap.ContainsKey(num))
                {
                    usersId.Add(num);
                }
                storage.component.UserId2EntityIdMap.Add(num, loadUsers.Entity.Id);
            }
            if (usersId.Count <= 0)
            {
                base.ScheduleEvent<UsersLoadedInternalEvent>(loadUsers);
            }
            else
            {
                LoadUsersEvent eventInstance = new LoadUsersEvent(loadUsers.Entity.Id, usersId);
                base.ScheduleEvent(eventInstance, session);
            }
        }

        [OnEventFire]
        public void ReSendUsersLoaded(UsersLoadedEvent e, SingleNode<ClientSessionComponent> session)
        {
            EntityRegistry entityRegistry = Flow.Current.EntityRegistry;
            if (entityRegistry.ContainsEntity(e.RequestEntityId))
            {
                base.ScheduleEvent<UsersLoadedInternalEvent>(entityRegistry.GetEntity(e.RequestEntityId));
            }
        }

        [OnEventFire]
        public void UnLoadUser(NodeRemoveEvent e, SingleNode<LoadUserComponent> loadUser, [JoinAll] SingleNode<SharedUsersStorageComponent> storage, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            storage.component.UserId2EntityIdMap.Remove(loadUser.component.UserId, loadUser.Entity.Id);
            if (!storage.component.UserId2EntityIdMap.ContainsKey(loadUser.component.UserId))
            {
                long[] userIds = new long[] { loadUser.component.UserId };
                UnLoadUsersEvent eventInstance = new UnLoadUsersEvent(userIds);
                base.ScheduleEvent(eventInstance, session);
            }
        }

        [OnEventFire]
        public void UnLoadUsers(NodeRemoveEvent e, SingleNode<LoadUsersComponent> loadUsers, [JoinAll] SingleNode<SharedUsersStorageComponent> storage, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            HashSet<long> userIds = new HashSet<long>();
            foreach (long num in loadUsers.component.UserIds)
            {
                storage.component.UserId2EntityIdMap.Remove(num, loadUsers.Entity.Id);
                if (!storage.component.UserId2EntityIdMap.ContainsKey(num))
                {
                    userIds.Add(num);
                }
            }
            if (userIds.Count > 0)
            {
                UnLoadUsersEvent eventInstance = new UnLoadUsersEvent(userIds);
                base.ScheduleEvent(eventInstance, session);
            }
        }

        [OnEventFire]
        public void UserLoaded(UsersLoadedInternalEvent e, SingleNode<LoadUserComponent> loadUsers)
        {
            loadUsers.Entity.AddComponent(new UserLoadedComponent(loadUsers.component.UserId));
        }

        [OnEventFire]
        public void UsersLoaded(UsersLoadedInternalEvent e, SingleNode<LoadUsersComponent> loadUsers)
        {
            loadUsers.Entity.AddComponent(new UsersLoadedComponent(loadUsers.component.UserIds));
        }

        public class UsersLoadedInternalEvent : Event
        {
        }
    }
}

