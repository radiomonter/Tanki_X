namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class LoginRewardAllItemsContainer : MonoBehaviour
    {
        public int currentDay;
        [SerializeField]
        private LoginRewardItemUI itemPrefab;
        [SerializeField]
        private LoginRewardDialog dialog;

        public void CheckLines()
        {
            LoginRewardItemUI[] componentsInChildren = base.GetComponentInChildren<ScrollRect>().GetComponentsInChildren<LoginRewardItemUI>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].SetupLines(i == 0, i == (componentsInChildren.Length - 1));
            }
        }

        public void Clear()
        {
            foreach (LoginRewardItemUI mui in base.GetComponentsInChildren<LoginRewardItemUI>(true))
            {
                mui.transform.SetParent(null);
                Destroy(mui.gameObject);
            }
        }

        public LoginRewardItemUI CreateDay(int day)
        {
            RectTransform content = base.GetComponentInChildren<ScrollRect>().content;
            LoginRewardItemUI mui = Instantiate<LoginRewardItemUI>(this.itemPrefab, content.transform);
            mui.Day = day;
            return mui;
        }

        public void InitItems(Dictionary<int, List<LoginRewardItem>> allRewards, int currentDay)
        {
            this.currentDay = currentDay;
            foreach (int num in allRewards.Keys)
            {
                LoginRewardItemUI mui = this.CreateDay(num);
                foreach (LoginRewardItem item in allRewards[num])
                {
                    Entity marketItemEntity = Flow.Current.EntityRegistry.GetEntity(item.MarketItemEntity);
                    if (!marketItemEntity.HasComponent<PremiumQuestItemComponent>())
                    {
                        mui.AddItem(marketItemEntity.GetComponent<ImageItemComponent>().SpriteUid, this.dialog.GetRewardItemNameWithAmount(marketItemEntity, item.Amount));
                    }
                }
                mui.fillType = (num != currentDay) ? ((currentDay <= num) ? LoginRewardProgressBar.FillType.Empty : LoginRewardProgressBar.FillType.Full) : LoginRewardProgressBar.FillType.Half;
                mui.gameObject.SetActive(true);
            }
            this.CheckLines();
        }

        public void ScrollToCurrentDay()
        {
            ScrollRect componentInChildren = base.GetComponentInChildren<ScrollRect>();
            componentInChildren.horizontalNormalizedPosition = Mathf.Max((float) 0f, (float) (((float) (this.currentDay - 2)) / ((float) componentInChildren.content.childCount)));
        }

        public void SetCurrentDay(int day)
        {
            Debug.Log($"Current day: {day}");
        }
    }
}

