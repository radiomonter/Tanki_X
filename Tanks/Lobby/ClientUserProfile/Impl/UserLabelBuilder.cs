namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct UserLabelBuilder
    {
        private GameObject userLabelInstance;
        public static GameObject userLabelPrefab;
        private bool withLeague;
        public UserLabelBuilder(long userId, GameObject userLabelInstance, string avatarId, bool premium)
        {
            this.userLabelInstance = userLabelInstance;
            userLabelInstance.GetComponent<UserLabelComponent>().UserId = userId;
            if (!string.IsNullOrEmpty(avatarId))
            {
                UserLabelAvatarComponent componentInChildren = userLabelInstance.GetComponentInChildren<UserLabelAvatarComponent>();
                componentInChildren.AvatarImage.SpriteUid = avatarId;
                componentInChildren.IsPremium = premium;
            }
            this.withLeague = false;
        }

        public static GameObject CreateDefaultLabel()
        {
            if (userLabelPrefab == null)
            {
                throw new Exception("User label prefab not found");
            }
            return Object.Instantiate<GameObject>(userLabelPrefab);
        }

        public UserLabelBuilder SetLeague(int league)
        {
            this.userLabelInstance.GetComponentInChildren<LeagueBorderComponent>().ImageListSkin.SelectedSpriteIndex = league;
            this.withLeague = true;
            return this;
        }

        public UserLabelBuilder SkipLoadUserFromServer()
        {
            this.userLabelInstance.GetComponent<UserLabelComponent>().SkipLoadUserFromServer = true;
            return this;
        }

        public UserLabelBuilder WithoutAvatar()
        {
            this.userLabelInstance.GetComponentInChildren<UserLabelAvatarComponent>().gameObject.SetActive(false);
            return this;
        }

        public UserLabelBuilder AllowInBattleIcon()
        {
            this.userLabelInstance.GetComponent<UserLabelComponent>().AllowInBattleIcon = true;
            return this;
        }

        public UserLabelBuilder SubscribeAvatarClick()
        {
            AddComponentToChildren<UserLabelAvatarComponent, UserLabelAvatarMappingComponent>(this.userLabelInstance);
            AddComponentToChildren<UserLabelAvatarComponent, CursorSwitcher>(this.userLabelInstance);
            return this;
        }

        public UserLabelBuilder SubscribeLevelClick()
        {
            AddComponentToChildren<RankIconComponent, UserLabelLevelMappingComponent>(this.userLabelInstance);
            AddComponentToChildren<RankIconComponent, CursorSwitcher>(this.userLabelInstance);
            return this;
        }

        public UserLabelBuilder SubscribeUidClick()
        {
            AddComponentToChildren<UidIndicatorComponent, UserLabelUidMappingComponent>(this.userLabelInstance);
            AddComponentToChildren<UidIndicatorComponent, CursorSwitcher>(this.userLabelInstance);
            return this;
        }

        public UserLabelBuilder SubscribeClick()
        {
            this.userLabelInstance.AddComponent<UserLabelMappingComponent>();
            AddComponentToChildren<UserLabelAvatarComponent, CursorSwitcher>(this.userLabelInstance);
            AddComponentToChildren<RankIconComponent, CursorSwitcher>(this.userLabelInstance);
            AddComponentToChildren<UidIndicatorComponent, CursorSwitcher>(this.userLabelInstance);
            return this;
        }

        public GameObject Build()
        {
            this.userLabelInstance.SetActive(true);
            Entity entity = this.userLabelInstance.GetComponent<EntityBehaviour>().Entity;
            UserLabelComponent component = this.userLabelInstance.GetComponent<UserLabelComponent>();
            if (!component.SkipLoadUserFromServer)
            {
                entity.AddComponent(new LoadUserComponent(component.UserId));
            }
            else if (!entity.HasComponent<UserGroupComponent>())
            {
                entity.AddComponent(new UserGroupComponent(component.UserId));
            }
            else if (entity.GetComponent<UserGroupComponent>().Key != component.UserId)
            {
                entity.RemoveComponent<UserGroupComponent>();
                entity.AddComponent(new UserGroupComponent(component.UserId));
            }
            LeagueBorderComponent componentInChildren = this.userLabelInstance.GetComponentInChildren<LeagueBorderComponent>();
            if (componentInChildren != null)
            {
                componentInChildren.gameObject.SetActive(this.withLeague);
            }
            return this.userLabelInstance;
        }

        private static void AddComponentToChildren<M, C>(GameObject userLabel) where M: MonoBehaviour where C: MonoBehaviour
        {
            GameObject gameObject = userLabel.GetComponentInChildren<M>().gameObject;
            if (gameObject.GetComponent<C>() == null)
            {
                gameObject.AddComponent<C>();
            }
        }

        private static void Unsubscribe<M, C>(GameObject userLabel) where M: MonoBehaviour where C: MonoBehaviour, Component, new()
        {
            Object.Destroy(userLabel.GetComponentInChildren<M>().gameObject.GetComponent<C>());
        }
    }
}

