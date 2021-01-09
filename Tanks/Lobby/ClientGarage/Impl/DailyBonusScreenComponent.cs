namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using TMPro;
    using UnityEngine.UI;

    public class DailyBonusScreenComponent : BehaviourComponent
    {
        public DailyBonusMapView mapView;
        public DailyBonusTeleportView teleportView;
        public TeleportHeaderView teleportHeaderView;
        public Button takeBonusButton;
        public Button takeContainerButton;
        public Button takeDetailTarget;
        public DailyBonusGarageItemReward completeDetailGarageItem;
        public CellsProgressBar cellsProgressBar;
        public LocalizedField noItemsFound;
        public LocalizedField itemsFound;
        public LocalizedField allItemsFound;
        public TextMeshProUGUI foundItemsLabel;
        private DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode;
        private DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode;
        private bool needUpdate;

        public void Awake()
        {
            this.takeBonusButton.interactable = this.mapView.selectedBonusElement != null;
            this.takeContainerButton.interactable = this.mapView.selectedBonusElement != null;
            this.takeDetailTarget.interactable = this.mapView.selectedBonusElement != null;
            this.mapView.onSelectedBonusElementChanged += new Action<MapViewBonusElement>(this.UpdateTakeBonusButtonInteractable);
            this.mapView.onSelectedBonusElementChanged += new Action<MapViewBonusElement>(this.teleportView.ViewSelectedBonus);
            this.teleportView.onStateChanged += new Action<DailyBonusTeleportState>(this.mapView.UpdateInteractable);
            this.mapView.UpdateInteractable(this.teleportView.State);
        }

        public void DisableAllButtons()
        {
            this.takeBonusButton.interactable = false;
            this.takeDetailTarget.interactable = false;
            this.takeContainerButton.interactable = false;
            base.GetComponentsInChildren<UpgradeTeleportButtonComponent>(true)[0].GetComponent<Button>().interactable = false;
            base.GetComponentsInChildren<GetNewTeleportButtonComponent>(true)[0].GetComponent<Button>().interactable = false;
        }

        private DailyBonusGarageItemReward GetCompleteUntakenDetailTargetItem(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode)
        {
            DailyBonusCycleComponent cycle = this.GetCycle(userDailyBonusNode, dailyBonusConfigNode);
            int num = cycle.Zones[(int) ((IntPtr) userDailyBonusNode.userDailyBonusZone.ZoneNumber)];
            DailyBonusData[] dailyBonuses = cycle.DailyBonuses;
            List<long> receivedRewards = userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards;
            for (int i = 0; i <= num; i++)
            {
                DailyBonusData data = dailyBonuses[i];
                if (receivedRewards.Contains(data.Code) && (data.DailyBonusType == DailyBonusType.DETAIL))
                {
                    DetailItem item = GarageItemsRegistry.GetItem<DetailItem>(data.DetailReward.MarketItemId);
                    if (item.Count == item.RequiredCount)
                    {
                        return data.DetailReward;
                    }
                }
            }
            return null;
        }

        public DailyBonusCycleComponent GetCycle(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) => 
            !userDailyBonusNode.userDailyBonusCycle.CycleNumber.Equals((long) 0L) ? ((DailyBonusCycleComponent) dailyBonusConfigNode.dailyBonusEndlessCycle) : ((DailyBonusCycleComponent) dailyBonusConfigNode.dailyBonusFirstCycle);

        public void Hide()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.gameObject.SetActive(false);
        }

        private void SetActiveOrUpgradableTeleportView(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode)
        {
            int zoneNumber = (int) userDailyBonusNode.userDailyBonusZone.ZoneNumber;
            if (this.UserTookAllBonusesInCurrentZone(userDailyBonusNode, dailyBonusConfigNode))
            {
                this.teleportView.SetUpgradableView(zoneNumber);
            }
            else
            {
                this.teleportView.SetActiveView(zoneNumber);
            }
        }

        public void Show()
        {
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
            base.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (this.needUpdate)
            {
                this.UpdateView();
                this.needUpdate = false;
            }
            if (InputMapping.Cancel)
            {
                this.Hide();
            }
            else if ((this.teleportView.State == DailyBonusTeleportState.Inactive) && (this.userDailyBonusNode.userDailyBonusNextReceivingDate.Date <= Date.Now))
            {
                this.SetActiveOrUpgradableTeleportView(this.userDailyBonusNode, this.dailyBonusConfigNode);
            }
        }

        private void UpdateFoundItemsLabel(DailyBonusCycleComponent cycle)
        {
            string allItemsFound = string.Empty;
            if (this.userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards.Count >= cycle.DailyBonuses.Length)
            {
                if (this.userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards.Count == cycle.DailyBonuses.Length)
                {
                    allItemsFound = (string) this.allItemsFound;
                }
            }
            else
            {
                allItemsFound = (string) this.itemsFound;
                if (this.userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards.Count == 0)
                {
                    allItemsFound = (string) this.noItemsFound;
                }
            }
            this.foundItemsLabel.SetText(allItemsFound.ToUpper());
        }

        private void UpdateTakeBonusButtonInteractable(MapViewBonusElement bonusElement)
        {
            this.takeContainerButton.gameObject.SetActive(true);
            this.takeBonusButton.gameObject.SetActive(true);
            bool flag = bonusElement != null;
            this.takeBonusButton.interactable = flag;
            this.takeContainerButton.interactable = flag;
            if (!ReferenceEquals(this.GetCompleteUntakenDetailTargetItem(this.userDailyBonusNode, this.dailyBonusConfigNode), null))
            {
                this.takeContainerButton.gameObject.SetActive(false);
                this.takeBonusButton.gameObject.SetActive(false);
                this.takeDetailTarget.interactable = true;
            }
            else if (!flag)
            {
                this.takeContainerButton.gameObject.SetActive(false);
            }
            else
            {
                bool flag3 = bonusElement.dailyBonusData.DailyBonusType == DailyBonusType.CONTAINER;
                this.takeContainerButton.gameObject.SetActive(flag3);
                this.takeBonusButton.gameObject.SetActive(!flag3);
            }
        }

        private void UpdateTeleport(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode)
        {
            int zoneNumber = (int) userDailyBonusNode.userDailyBonusZone.ZoneNumber;
            this.completeDetailGarageItem = this.GetCompleteUntakenDetailTargetItem(userDailyBonusNode, dailyBonusConfigNode);
            if (this.completeDetailGarageItem != null)
            {
                this.teleportView.SetDetailTargetView(zoneNumber, this.completeDetailGarageItem);
                this.takeDetailTarget.gameObject.SetActive(true);
                this.takeBonusButton.gameObject.SetActive(false);
                this.takeContainerButton.gameObject.SetActive(false);
                this.takeDetailTarget.interactable = true;
            }
            else
            {
                this.takeDetailTarget.gameObject.SetActive(false);
                this.UpdateTakeBonusButtonInteractable(this.mapView.selectedBonusElement);
                if (this.UserTookAllBonuses(userDailyBonusNode, dailyBonusConfigNode))
                {
                    this.teleportView.SetBrokenView();
                    this.teleportHeaderView.SetBrokenView();
                }
                else
                {
                    this.teleportHeaderView.UpdateView(zoneNumber);
                    Date endDate = userDailyBonusNode.userDailyBonusNextReceivingDate.Date;
                    if (endDate <= Date.Now)
                    {
                        this.SetActiveOrUpgradableTeleportView(userDailyBonusNode, dailyBonusConfigNode);
                    }
                    else
                    {
                        this.teleportView.SetInactiveState(zoneNumber, endDate, ((float) userDailyBonusNode.userDailyBonusNextReceivingDate.TotalMillisLength) / 1000f);
                    }
                }
            }
        }

        private void UpdateView()
        {
            DailyBonusCycleComponent cycle = this.GetCycle(this.userDailyBonusNode, this.dailyBonusConfigNode);
            this.cellsProgressBar.Init(cycle.DailyBonuses.Length, cycle.DailyBonuses, this.userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards);
            this.mapView.UpdateView(cycle, this.userDailyBonusNode);
            this.UpdateTeleport(this.userDailyBonusNode, this.dailyBonusConfigNode);
            this.UpdateFoundItemsLabel(cycle);
        }

        public void UpdateView(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode)
        {
            this.userDailyBonusNode = userDailyBonusNode;
            this.dailyBonusConfigNode = dailyBonusConfigNode;
            this.UpdateView();
        }

        public void UpdateViewInNextFrame(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode)
        {
            this.userDailyBonusNode = userDailyBonusNode;
            this.dailyBonusConfigNode = dailyBonusConfigNode;
            this.needUpdate = true;
        }

        private bool UserTookAllBonuses(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) => 
            userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards.Count.Equals(this.GetCycle(userDailyBonusNode, dailyBonusConfigNode).DailyBonuses.Length);

        private bool UserTookAllBonusesInCurrentZone(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode)
        {
            DailyBonusCycleComponent cycle = this.GetCycle(userDailyBonusNode, dailyBonusConfigNode);
            int num = cycle.Zones[(int) ((IntPtr) userDailyBonusNode.userDailyBonusZone.ZoneNumber)];
            DailyBonusData[] dailyBonuses = cycle.DailyBonuses;
            List<long> receivedRewards = userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards;
            for (int i = 0; i <= num; i++)
            {
                if (!receivedRewards.Contains(dailyBonuses[i].Code))
                {
                    return false;
                }
            }
            return true;
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

