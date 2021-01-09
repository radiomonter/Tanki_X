namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using TMPro;
    using UnityEngine;

    public class LeagueRewardUIComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI currentLeagueTitle;
        [SerializeField]
        private LeagueRewardListUIComponent leagueChestList;
        [SerializeField]
        private LeagueRewardListUIComponent seasonRewardList;
        [SerializeField]
        private TextMeshProUGUI leaguePoints;
        [SerializeField]
        private LocalizedField leaguePointsCurrentMax;
        [SerializeField]
        private LocalizedField leaguePlaceCurrentMax;
        [SerializeField]
        private LocalizedField leaguePointsCurrent;
        [SerializeField]
        private LocalizedField leaguePointsNotCurrent;
        [SerializeField]
        private LocalizedField leagueAccusative;
        [SerializeField]
        private LocalizedField seasonEndsIn;
        [SerializeField]
        private LocalizedField seasonEndsDays;
        [SerializeField]
        private LocalizedField seasonEndsHours;
        [SerializeField]
        private LocalizedField seasonEndsMinutes;
        [SerializeField]
        private LocalizedField seasonEndsSeconds;
        [SerializeField]
        private TextMeshProUGUI seasonEndsInText;
        [SerializeField]
        private GameObject seasonRewardsTitleLayout;
        [SerializeField]
        private LocalizedField chestTooltipLocalization;
        [SerializeField]
        private LocalizedField chestTooltipLowLeagueLocalization;
        [SerializeField]
        private TooltipShowBehaviour chestTooltip;
        [SerializeField]
        private GameObject tabsPanel;
        private int selectedBar;
        private long chestScoreLimit;
        private int leaguesCount;
        private Entity userLeague;
        private double currentUserReputation;
        private readonly Dictionary<long, double> lastUserReputationInLeagues = new Dictionary<long, double>();
        private LeagueCarouselUIComponent carousel;

        private void AddItemToList(DroppedItem item, LeagueRewardListUIComponent list)
        {
            this.AddItemToList(item.marketItemEntity.Id, item.Amount, list);
        }

        private void AddItemToList(long itemId, int count, LeagueRewardListUIComponent list)
        {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(itemId);
            DescriptionItemComponent component = entity.GetComponent<DescriptionItemComponent>();
            ImageItemComponent component2 = entity.GetComponent<ImageItemComponent>();
            string str = (count <= 1) ? string.Empty : (" x" + count);
            string str2 = string.Empty;
            if (!entity.HasComponent<ContainerMarkerComponent>())
            {
                str2 = MarketItemNameLocalization.Instance.GetCategoryName(entity) + " ";
            }
            string[] textArray1 = new string[] { str2, component.Name, str, "\n", component.Description };
            list.AddItem(string.Concat(textArray1), component2.SpriteUid);
        }

        public LeagueTitleUIComponent AddLeagueItem(Entity entity) => 
            this.Carousel.AddLeagueItem(entity);

        private void FillInfo(LeagueTitleUIComponent selectedLeague)
        {
            bool flag = selectedLeague.LeagueEntity.Equals(this.userLeague);
            this.currentLeagueTitle.color = !flag ? Color.clear : Color.white;
            string nameAccusative = string.Empty;
            double d = 0.0;
            int leagueIndex = selectedLeague.LeagueEntity.GetComponent<LeagueConfigComponent>().LeagueIndex;
            GetLeagueByIndexEvent eventInstance = new GetLeagueByIndexEvent(leagueIndex + 1);
            base.ScheduleEvent(eventInstance, selectedLeague.LeagueEntity);
            Entity leagueEntity = eventInstance.leagueEntity;
            if (leagueEntity != null)
            {
                nameAccusative = leagueEntity.GetComponent<LeagueNameComponent>().NameAccusative;
                d = this.GetReputationToEnter(leagueEntity);
            }
            if (flag)
            {
                this.leaguePoints.text = !leagueIndex.Equals((int) (this.leaguesCount - 1)) ? string.Format(this.leaguePointsCurrent.Value, "<color=white><b>" + Math.Truncate(this.currentUserReputation), Math.Truncate(d) + "</b></color>", nameAccusative + " " + this.leagueAccusative.Value) : (string.Format(this.leaguePointsCurrentMax.Value, this.ToBoldText(Math.Truncate(this.currentUserReputation).ToString())) + "\n" + string.Format(this.leaguePlaceCurrentMax.Value, this.ToBoldText(this.PlaceInTopLeague.ToString())));
            }
            else
            {
                double a = this.GetReputationToEnter(selectedLeague.LeagueEntity) - this.currentUserReputation;
                this.leaguePoints.text = (a <= 0.0) ? string.Empty : string.Format(this.leaguePointsNotCurrent.Value, "<color=white><b>" + Math.Ceiling(a) + "</b></color>");
            }
        }

        private void FillLeagueChest(LeagueTitleUIComponent selectedLeague)
        {
            this.leagueChestList.Clear();
            long chestId = selectedLeague.LeagueEntity.GetComponent<ChestBattleRewardComponent>().ChestId;
            this.AddItemToList(chestId, 1, this.leagueChestList);
        }

        private void FillSeasonReward(LeagueTitleUIComponent selectedLeague)
        {
            this.seasonRewardList.Clear();
            bool flag = false;
            bool flag2 = false;
            if (!selectedLeague.LeagueEntity.HasComponent<CurrentSeasonRewardForClientComponent>())
            {
                Debug.LogWarning("League doesn't have reward!");
            }
            else
            {
                List<EndSeasonRewardItem> rewards = selectedLeague.LeagueEntity.GetComponent<CurrentSeasonRewardForClientComponent>().Rewards;
                if (rewards.Count <= 0)
                {
                    Debug.LogWarning("End season rewards is empty");
                }
                else
                {
                    if (this.selectedBar > (rewards.Count - 1))
                    {
                        this.selectedBar = 0;
                        this.SetRadioButton(0);
                    }
                    flag2 = rewards.Count > 1;
                    List<DroppedItem> items = rewards[this.selectedBar].Items;
                    if (items != null)
                    {
                        flag = items.Count > 0;
                        foreach (DroppedItem item in items)
                        {
                            this.AddItemToList(item, this.seasonRewardList);
                        }
                    }
                }
                this.seasonRewardsTitleLayout.SetActive(flag);
                this.tabsPanel.SetActive(flag2);
            }
        }

        private void FillTooltip(LeagueTitleUIComponent selectedLeague)
        {
            string str = string.Format(this.chestTooltipLocalization.Value, this.chestScoreLimit);
            if (this.userLeague.GetComponent<LeagueConfigComponent>().LeagueIndex < 3)
            {
                str = str + this.chestTooltipLowLeagueLocalization.Value;
            }
            this.chestTooltip.TipText = str;
        }

        private double GetReputationToEnter(Entity league)
        {
            double reputationToEnter = league.GetComponent<LeagueConfigComponent>().ReputationToEnter;
            return (!this.lastUserReputationInLeagues.ContainsKey(league.Id) ? reputationToEnter : Math.Max(this.lastUserReputationInLeagues[league.Id], reputationToEnter));
        }

        public string GetSeasonEndsAsString(Date endDate)
        {
            float unityTime = endDate.UnityTime;
            float num2 = Date.Now.UnityTime;
            float num3 = (num2 >= unityTime) ? 0f : (unityTime - num2);
            int num4 = Mathf.FloorToInt(num3 / 86400f);
            int num5 = Mathf.FloorToInt((num3 - (((num4 * 0x18) * 60) * 60)) / 3600f);
            int num6 = Mathf.FloorToInt(((num3 - (((num4 * 0x18) * 60) * 60)) - ((num5 * 60) * 60)) / 60f);
            int num7 = Mathf.FloorToInt(((num3 - (((num4 * 0x18) * 60) * 60)) - ((num5 * 60) * 60)) - (num6 * 60));
            string seasonEndsIn = (string) this.seasonEndsIn;
            return ((num4 <= 0) ? ((num5 <= 0) ? ((seasonEndsIn + num6 + this.seasonEndsMinutes) + num7 + this.seasonEndsSeconds) : ((seasonEndsIn + num5 + this.seasonEndsHours) + num6 + this.seasonEndsMinutes)) : ((seasonEndsIn + num4 + this.seasonEndsDays) + num5 + this.seasonEndsHours));
        }

        private void LeagueSelected(LeagueTitleUIComponent selectedLeague)
        {
            this.FillInfo(selectedLeague);
            this.FillLeagueChest(selectedLeague);
            this.FillSeasonReward(selectedLeague);
            this.FillTooltip(selectedLeague);
        }

        private void OnDestroy()
        {
            LeagueCarouselUIComponent carousel = this.Carousel;
            carousel.itemSelected -= new CarouselItemSelected(this.LeagueSelected);
        }

        private void OnEnable()
        {
            this.SetRadioButton(0);
        }

        public void PutReputationToEnter(long legueId, double reputation)
        {
            this.lastUserReputationInLeagues[legueId] = reputation;
        }

        public void SelectBar(int value)
        {
            this.selectedBar = value;
            this.FillSeasonReward(this.Carousel.CurrentLeague);
            this.SetRadioButton(value);
        }

        public void SelectUserLeague(Entity entity, double userReputation)
        {
            this.userLeague = entity;
            this.currentUserReputation = userReputation;
            this.Carousel.SelectItem(entity);
        }

        public void SetChestScoreLimit(long score)
        {
            this.chestScoreLimit = score;
        }

        public void SetLeaguesCount(int count)
        {
            this.leaguesCount = count;
        }

        private void SetRadioButton(int value)
        {
            RadioButton[] componentsInChildren = base.GetComponentsInChildren<RadioButton>(true);
            if (componentsInChildren.Length > value)
            {
                componentsInChildren[(componentsInChildren.Length - 1) - value].Activate();
            }
        }

        public void SetSeasonEndDate(Date endDate)
        {
            string seasonEndsAsString = this.GetSeasonEndsAsString(endDate);
            this.SetSeasonEndsInText(seasonEndsAsString);
        }

        public void SetSeasonEndsInText(string endsIn)
        {
            this.seasonEndsInText.text = endsIn;
        }

        private string ToBoldText(string text) => 
            "<color=white><b>" + text + "</b></color>";

        public void UpdateLeagueRewardUI()
        {
            this.FillInfo(this.Carousel.CurrentLeague);
        }

        public int PlaceInTopLeague { get; set; }

        public LeagueCarouselUIComponent Carousel
        {
            get
            {
                if (this.carousel == null)
                {
                    this.carousel = base.GetComponentInChildren<LeagueCarouselUIComponent>(true);
                    this.carousel.itemSelected += new CarouselItemSelected(this.LeagueSelected);
                }
                return this.carousel;
            }
        }
    }
}

