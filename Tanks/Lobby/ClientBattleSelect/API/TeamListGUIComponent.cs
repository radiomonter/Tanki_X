namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;

    public class TeamListGUIComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject joinButton;
        [SerializeField]
        private RectTransform joinButtonContainer;
        [SerializeField]
        private LobbyUserListItemComponent userListItemPrefab;
        [SerializeField]
        private LobbyUserListItemComponent customLobbyuserListItemPrefab;
        [SerializeField]
        private RectTransform scrollViewRect;
        [SerializeField]
        private TextMeshProUGUI membersCount;
        private bool showSearchingText = true;
        private int maxCount = 20;
        [CompilerGenerated]
        private static Func<LobbyUserListItemComponent, bool> <>f__am$cache0;

        public void AddUser(Entity userEntity, bool selfUser, bool customLobby)
        {
            if (this.GetItemByUserEntity(userEntity) == null)
            {
                this.InitNewItem(userEntity, selfUser, customLobby).transform.SetSiblingIndex(this.GetFirstEmptyIndex());
                this.UpdateList();
            }
        }

        public void Clean()
        {
            LobbyUserListItemComponent[] componentsInChildren = this.scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].transform.SetParent(base.transform);
                Destroy(componentsInChildren[i].gameObject);
            }
        }

        private void FillEmptyCells()
        {
            LobbyUserListItemComponent[] componentsInChildren = this.scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
            int num = this.maxCount - componentsInChildren.Length;
            for (int i = 0; i < num; i++)
            {
                this.InitNewItem(null, false, false);
            }
        }

        private int GetFirstEmptyIndex()
        {
            LobbyUserListItemComponent[] componentsInChildren = this.scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (componentsInChildren[i].Empty)
                {
                    return i;
                }
            }
            return componentsInChildren.Length;
        }

        public GameObject GetItemByUserEntity(Entity userEntity)
        {
            LobbyUserListItemComponent[] componentsInChildren = this.scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (!componentsInChildren[i].Empty && componentsInChildren[i].userEntity.Equals(userEntity))
                {
                    return componentsInChildren[i].gameObject;
                }
            }
            return null;
        }

        private int GetUsersCount()
        {
            LobbyUserListItemComponent[] componentsInChildren = this.scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (componentsInChildren[i].Empty)
                {
                    return i;
                }
            }
            return componentsInChildren.Length;
        }

        public bool HasEmptyCells()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = uli => uli.userInfo.activeSelf;
            }
            int num = this.scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>().Count<LobbyUserListItemComponent>(<>f__am$cache0);
            return (this.maxCount > num);
        }

        private GameObject InitNewItem(Entity userEntity, bool selfUser, bool customLobby)
        {
            LobbyUserListItemComponent component = Instantiate<LobbyUserListItemComponent>((customLobby || selfUser) ? this.customLobbyuserListItemPrefab : this.userListItemPrefab).GetComponent<LobbyUserListItemComponent>();
            component.transform.SetParent(this.scrollViewRect, false);
            component.userEntity = userEntity;
            component.selfUser = selfUser;
            component.ShowSearchingText = this.ShowSearchingText;
            component.gameObject.SetActive(true);
            return component.gameObject;
        }

        private void MoveJoinButton(bool interactable)
        {
            float num = Mathf.Min(this.scrollViewRect.rect.height, base.GetComponent<RectTransform>().rect.height) + 20f;
            this.joinButtonContainer.anchoredPosition = new Vector2(this.joinButtonContainer.anchoredPosition.x, -num);
            this.joinButton.GetComponent<Button>().interactable = interactable;
        }

        private void OnEnable()
        {
            this.UpdateList();
            base.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1f;
        }

        private void RemoveExcessItems()
        {
            LobbyUserListItemComponent[] componentsInChildren = this.scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
            for (int i = this.maxCount; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].transform.SetParent(base.transform);
                Destroy(componentsInChildren[i].gameObject);
            }
        }

        public void RemoveUser(Entity userEntity)
        {
            GameObject itemByUserEntity = this.GetItemByUserEntity(userEntity);
            if (itemByUserEntity != null)
            {
                itemByUserEntity.transform.SetParent(base.transform);
                Destroy(itemByUserEntity);
                this.UpdateList();
            }
        }

        private void SortBySquadGroup()
        {
            LobbyUserListItemComponent[] componentsInChildren = this.scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
            List<long> list = new List<long>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (!componentsInChildren[i].Empty && (componentsInChildren[i].userEntity.HasComponent<SquadGroupComponent>() && !list.Contains(componentsInChildren[i].userEntity.GetComponent<SquadGroupComponent>().Key)))
                {
                    list.Add(componentsInChildren[i].userEntity.GetComponent<SquadGroupComponent>().Key);
                }
            }
            int num2 = 0;
            while (num2 < list.Count)
            {
                int index = 0;
                while (true)
                {
                    if (index >= componentsInChildren.Length)
                    {
                        num2++;
                        break;
                    }
                    if (!componentsInChildren[index].Empty && (componentsInChildren[index].userEntity.HasComponent<SquadGroupComponent>() && (componentsInChildren[index].userEntity.GetComponent<SquadGroupComponent>().Key == list[num2])))
                    {
                        componentsInChildren[index].transform.SetSiblingIndex(0);
                    }
                    index++;
                }
            }
        }

        private void UpdateBounds()
        {
            float y = (this.userListItemPrefab.GetComponent<LayoutElement>().preferredHeight + this.scrollViewRect.GetComponent<VerticalLayoutGroup>().spacing) * this.scrollViewRect.childCount;
            this.scrollViewRect.sizeDelta = new Vector2(this.scrollViewRect.sizeDelta.x, y);
        }

        private void UpdateList()
        {
            this.RemoveExcessItems();
            this.FillEmptyCells();
            this.UpdateBounds();
            this.UpdateMembersCount();
            this.MoveJoinButton(this.HasEmptyCells());
            this.SortBySquadGroup();
        }

        private void UpdateMembersCount()
        {
            this.membersCount.text = this.GetUsersCount() + "/" + this.maxCount.ToString();
        }

        public bool ShowSearchingText
        {
            get => 
                this.showSearchingText;
            set
            {
                this.showSearchingText = value;
                LobbyUserListItemComponent[] componentsInChildren = this.scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].ShowSearchingText = this.showSearchingText;
                }
            }
        }

        public int MaxCount
        {
            get => 
                this.maxCount;
            set
            {
                this.maxCount = value;
                this.UpdateList();
            }
        }

        public bool ShowJoinButton
        {
            set => 
                this.joinButton.SetActive(value);
        }
    }
}

