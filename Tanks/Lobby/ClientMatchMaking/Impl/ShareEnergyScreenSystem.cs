namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class ShareEnergyScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void ActivateStartBattleButton(NodeRemoveEvent e, SingleNode<NotAllowedToBattleEntranceComponent> notAllowedToBattleEntrance, [JoinAll] SingleNode<StartSquadBattleButtonComponent> button)
        {
            button.component.GetComponent<Button>().interactable = true;
        }

        [OnEventFire]
        public void BuyEnergy(DialogConfirmEvent e, SingleNode<BuyEnergyDialogComponent> dialog)
        {
            PressEnergyContextBuyButtonEvent eventInstance = new PressEnergyContextBuyButtonEvent {
                Count = dialog.component.EnergyCount,
                XPrice = dialog.component.Price
            };
            base.ScheduleEvent(eventInstance, dialog);
        }

        [OnEventFire]
        public void CreateUser(NodeAddedEvent e, SingleNode<UsersEnergyCellsListUIComponent> list, [Combine] UserInSquadNode userInSquad, [JoinBySquad] SelfUserInSquadNode selfUser)
        {
            UserEnergyCellUIComponent component = list.component.AddUserCell();
            userInSquad.userGroup.Attach(component.GetComponent<EntityBehaviour>().Entity);
            this.UpdateSquadTeleportInfo(userInSquad);
        }

        [OnEventFire]
        public void DeactivateStartBattleButton(NodeAddedEvent e, SingleNode<NotAllowedToBattleEntranceComponent> notAllowedToBattleEntrance, [JoinAll] SingleNode<StartSquadBattleButtonComponent> button)
        {
            button.component.GetComponent<Button>().interactable = false;
        }

        [OnEventFire]
        public void EnergyAdded(NodeAddedEvent e, EnergyItemNode energy, [JoinByUser] UserInSquadNode user, [JoinByUser] UserEnergyCellNode userCell, [JoinByUser] UserInSquadNode userInSquad, [JoinByLeague] LeagueNode league)
        {
            this.UpdateEnergyCell(user, userCell, energy, league);
            this.UpdateSquadTeleportInfo(user);
        }

        [OnEventFire]
        public void GetEnergyPriceEvent(EnergyPriceEvent e, UserInSquadNode userInSquad, [JoinByUser] EnergyItemNode energy, [JoinByMarketItem] SingleNode<XPriceItemComponent> priceNode)
        {
            long num = (long) Math.Ceiling((double) ((priceNode.component.Price * e.count) / ((double) priceNode.component.Pieces)));
            e.price = num;
        }

        public long GetPayed(UserInSquadNode userInSquad)
        {
            long num = 0L;
            foreach (long num2 in userInSquad.battleEntrancePayer.EnergyPayments.Keys)
            {
                if (num2 != userInSquad.Entity.Id)
                {
                    num += userInSquad.battleEntrancePayer.EnergyPayments[num2];
                }
            }
            return num;
        }

        [OnEventFire]
        public void GetSelfExcessEnergy(SelfExcessEnergyEvent e, Node any, [JoinAll] SelfUserInSquadNode selfUserInSquad, [JoinByUser] EnergyItemNode selfEnergy, [JoinByUser] SelfUserInSquadNode selfUserInSquad1, [JoinByLeague] LeagueNode selfLeague)
        {
            e.ExcessEnergy = (selfEnergy.userItemCounter.Count - selfLeague.leagueEnergyConfig.Cost) - this.GetPayed(selfUserInSquad);
        }

        [OnEventFire]
        public void GetSquadCurrentEnergy(SquadCurrentEnergy e, UserInSquadNode userInSquad, [JoinBySquad, Combine] UserInSquadNode user, [JoinByUser] EnergyItemNode userEnergy, [JoinByUser] UserInSquadNode user1, [JoinByLeague] LeagueNode league)
        {
            UserGiftEnergyEvent eventInstance = new UserGiftEnergyEvent();
            base.ScheduleEvent(eventInstance, user);
            long num = (long) Mathf.Min((float) league.leagueEnergyConfig.Cost, (float) (userEnergy.userItemCounter.Count + eventInstance.totalGift));
            e.CurrentEnergy += num;
        }

        [OnEventFire]
        public void GetSquadTeleportPrice(SquadTeleportPriceEvent e, UserInSquadNode userInSquad, [JoinBySquad, Combine] UserInSquadNode user, [JoinByLeague] LeagueNode league)
        {
            e.TeleportPrice += league.leagueEnergyConfig.Cost;
        }

        [OnEventFire]
        public void GetUserGiftEnergy(UserGiftEnergyEvent e, UserInSquadNode user, [JoinBySquad] ICollection<UserInSquadNode> users)
        {
            foreach (UserInSquadNode node in users)
            {
                if (!ReferenceEquals(node, user))
                {
                    Dictionary<long, long> energyPayments = node.battleEntrancePayer.EnergyPayments;
                    foreach (long num in energyPayments.Keys)
                    {
                        if (num == user.Entity.Id)
                        {
                            e.uids.Add(node.userUid.Uid);
                            e.totalGift += energyPayments[num];
                        }
                    }
                }
            }
        }

        [OnEventFire]
        public void HideAdditionalTeleportValueForUserCell(NodeRemoveEvent e, EnergyPreviewCellNode previewCell, [JoinAll] SingleNode<ShareEnergyScreenComponent> shareEnergyDialog)
        {
            this.UpdateSquadTeleportInfo(previewCell);
        }

        [OnEventFire]
        public void HideAllShareButtons(HideAllShareButtonsEvent e, Node any, [JoinAll] ICollection<UserEnergyCellNode> cells)
        {
            foreach (UserEnergyCellNode node in cells)
            {
                node.userEnergyCellUi.HideShareButton();
            }
        }

        [OnEventFire]
        public void InitShareButton(NodeAddedEvent e, SingleNode<ShareEnergyButtonComponent> button)
        {
            if (button.Entity.HasComponent<UserGroupComponent>())
            {
                button.Entity.RemoveComponent<UserGroupComponent>();
            }
            button.component.GetComponentInParent<UserEnergyCellUIComponent>().GetComponent<EntityBehaviour>().Entity.GetComponent<UserGroupComponent>().Attach(button.Entity);
        }

        [OnEventFire]
        public void InitStartBattleButton(NodeAddedEvent e, SingleNode<StartSquadBattleButtonComponent> button, [JoinAll] SelfUserInSquadNode selfUserInSquad, [JoinBySquad] SquadNode squad)
        {
            button.component.GetComponent<Button>().interactable = !squad.Entity.HasComponent<NotAllowedToBattleEntranceComponent>();
        }

        [OnEventFire]
        public void InitUser(NodeAddedEvent e, UserEnergyCellNode userEnergyCell, [JoinByUser] UserInSquadNode user, [JoinByLeague] LeagueNode league, UserEnergyCellNode userEnergyCell1, [JoinByUser] Optional<EnergyItemNode> energy)
        {
            UserGiftEnergyEvent eventInstance = new UserGiftEnergyEvent();
            base.ScheduleEvent(eventInstance, user);
            userEnergyCell.userEnergyCellUi.Setup(user.userUid.Uid, !energy.IsPresent() ? 0L : (energy.Get().userItemCounter.Count + eventInstance.totalGift), league.leagueEnergyConfig.Cost);
            userEnergyCell.userEnergyCellUi.SetGiftValue(eventInstance.totalGift, eventInstance.uids);
        }

        [OnEventFire]
        public void RemoveUser(NodeRemoveEvent e, UserInSquadNode userInSquad, [JoinByUser] SingleNode<UserEnergyCellUIComponent> userCell, [JoinAll] SingleNode<UsersEnergyCellsListUIComponent> list)
        {
            list.component.RemoveUserCell(userCell.component);
            this.UpdateSquadTeleportInfo(userInSquad);
        }

        [OnEventFire]
        public void SetEnergy(NodeAddedEvent e, UserEnergyBarNode screen, [JoinAll] SelfUserInSquadNode user)
        {
            base.ScheduleEvent<UpdateSelfUserEnergyEvent>(user);
        }

        [OnEventFire]
        public void SetEnergy(UpdateClientEnergyEvent e, EnergyItemNode energy, [JoinByUser] UserInSquadNode user, [JoinAll] SelfUserInSquadNode selfUser)
        {
            base.ScheduleEvent<UpdateSelfUserEnergyEvent>(selfUser);
        }

        [OnEventFire]
        public void SetReadyPlayers(UpdateTeleportInfoEvent e, Node any, [JoinAll] SelfUserInSquadNode selfUserInSquad, [JoinAll] ICollection<UserEnergyCellNode> userCells, [JoinAll] SingleNode<ShareEnergyScreenComponent> shareEnergyDialog)
        {
            int ready = 0;
            foreach (UserEnergyCellNode node in userCells)
            {
                if (node.userEnergyCellUi.Ready)
                {
                    ready++;
                }
            }
            shareEnergyDialog.component.ReadyPlayers(ready, userCells.Count);
        }

        [OnEventFire]
        public void SetSelfEnergy(UpdateSelfUserEnergyEvent e, SelfUserInSquadNode user, [JoinByUser] EnergyItemNode energy, SelfUserInSquadNode user1, [JoinByLeague] LeagueNode league, [JoinAll] UserEnergyBarNode screen)
        {
            UserGiftEnergyEvent eventInstance = new UserGiftEnergyEvent();
            base.ScheduleEvent(eventInstance, user);
            screen.userEnergyBarUi.SetEnergyLevel((energy.userItemCounter.Count - this.GetPayed(user)) + eventInstance.totalGift, league.leagueEnergyConfig.Capacity);
        }

        [OnEventFire]
        public void SetSquadLeaderView(NodeAddedEvent e, SingleNode<ShareEnergyScreenComponent> dialog, [JoinAll] Optional<SelfSquadLeaderNode> selfSquadLeader)
        {
            dialog.component.SelfPlayerIsSquadLeader = selfSquadLeader.IsPresent();
        }

        [OnEventFire]
        public void SetSquadLeaderView(NodeRemoveEvent e, SelfSquadLeaderNode selfSquadLeader, [JoinAll] SingleNode<ShareEnergyScreenComponent> dialog)
        {
            dialog.component.SelfPlayerIsSquadLeader = false;
        }

        [OnEventFire]
        public void SetSquadLeaferView(NodeAddedEvent e, SelfSquadLeaderNode selfSquadLeader, [JoinAll] SingleNode<ShareEnergyScreenComponent> dialog)
        {
            dialog.component.SelfPlayerIsSquadLeader = true;
        }

        [OnEventFire]
        public void ShareEnergy(ButtonClickEvent e, SingleNode<ShareEnergyButtonComponent> button, [JoinByUser] UserInSquadNode userInSquad, [JoinByUser] UserEnergyCellNode userCell, [JoinAll] SelfUserInSquadNode selfUserInSquad, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            userCell.userEnergyCellUi.HideShareButton();
            if (!userCell.userEnergyCellUi.Buy)
            {
                ShareEnergyEvent eventInstance = new ShareEnergyEvent {
                    ReceiverId = userInSquad.Entity.Id
                };
                base.ScheduleEvent(eventInstance, selfUserInSquad);
            }
            else
            {
                EnergyPriceEvent eventInstance = new EnergyPriceEvent {
                    count = userCell.userEnergyCellUi.ShareEnergyValue
                };
                base.ScheduleEvent(eventInstance, userInSquad);
                dialogs.component.Get<BuyEnergyDialogComponent>().Show(eventInstance.count, eventInstance.price);
            }
        }

        [OnEventFire]
        public void ShowAdditionalTeleportValueForUserCell(NodeAddedEvent e, EnergyPreviewCellNode previewCell)
        {
            this.UpdateSquadTeleportInfo(previewCell);
        }

        [OnEventComplete]
        public void SquadEnergyChanged(SquadEnergyChangedEvent e, SquadNode squad, [JoinBySquad] SelfUserInSquadNode selfUserInSquad)
        {
            this.UpdateSquadTeleportInfo(selfUserInSquad);
        }

        private void UpdateEnergyCell(UserInSquadNode user, [JoinByUser] UserEnergyCellNode userCell, [JoinByUser] EnergyItemNode energy, [JoinByLeague] LeagueNode league)
        {
            UserGiftEnergyEvent eventInstance = new UserGiftEnergyEvent();
            base.ScheduleEvent(eventInstance, user);
            userCell.userEnergyCellUi.Setup(user.userUid.Uid, energy.userItemCounter.Count + eventInstance.totalGift, league.leagueEnergyConfig.Cost);
            userCell.userEnergyCellUi.SetGiftValue(eventInstance.totalGift, eventInstance.uids);
        }

        [OnEventFire]
        public void UpdateEnergyCell(UpdateClientEnergyEvent e, EnergyItemNode energy, [JoinByUser] UserInSquadNode user, [JoinByUser] UserEnergyCellNode userCell, [JoinByUser] UserInSquadNode userInSquad, [JoinByLeague] LeagueNode league)
        {
            this.UpdateEnergyCell(user, userCell, energy, league);
            this.UpdateSquadTeleportInfo(user);
        }

        [OnEventFire]
        public void UpdateEnergyCells(SquadEnergyChangedEvent e, SquadNode squad, [JoinBySquad] SelfUserInSquadNode selfUserInSquad, [JoinBySquad, Combine] UserInSquadNode user, [JoinByUser] UserEnergyCellNode userCell, [JoinByUser] EnergyItemNode energy, [JoinByUser] UserInSquadNode userInSquad, [JoinByLeague] LeagueNode league)
        {
            this.UpdateEnergyCell(user, userCell, energy, league);
        }

        private void UpdateSquadTeleportInfo(Node any)
        {
            base.NewEvent<UpdateTeleportInfoEvent>().Attach(any).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UpdateSquadTeleportInfo(UpdateTeleportInfoEvent e, Node any, [JoinAll] SelfUserInSquadNode selfUserInSquad, [JoinAll] SingleNode<ShareEnergyScreenComponent> shareEnergyDialog, [JoinAll] UserEnergyBarNode userEnergyBar, [JoinAll] Optional<EnergyPreviewCellNode> previewCell, [JoinByUser] Optional<EnergyItemNode> previewUserEnergy, [JoinByUser] Optional<UserInSquadNode> previewUserInSquad, [JoinByLeague] Optional<LeagueNode> previewUserLeague)
        {
            SquadTeleportPriceEvent eventInstance = new SquadTeleportPriceEvent();
            base.ScheduleEvent(eventInstance, selfUserInSquad);
            SelfExcessEnergyEvent event3 = new SelfExcessEnergyEvent();
            base.ScheduleEvent(event3, selfUserInSquad);
            SquadCurrentEnergy energy = new SquadCurrentEnergy();
            base.ScheduleEvent(energy, selfUserInSquad);
            shareEnergyDialog.component.TeleportPriceProgressBar.Progress = ((float) energy.CurrentEnergy) / ((float) eventInstance.TeleportPrice);
            base.ScheduleEvent<UpdateSelfUserEnergyEvent>(selfUserInSquad);
            long num = 0L;
            if (previewCell.IsPresent())
            {
                bool flag = previewUserInSquad.Get().Entity.HasComponent<SelfUserComponent>();
                UserGiftEnergyEvent event4 = new UserGiftEnergyEvent();
                base.ScheduleEvent(event4, previewUserInSquad.Get());
                num = (previewUserLeague.Get().leagueEnergyConfig.Cost - previewUserEnergy.Get().userItemCounter.Count) - event4.totalGift;
                if (num <= 0L)
                {
                    previewCell.Get().userEnergyCellUi.HideShareButton();
                }
                else if (flag || (event3.ExcessEnergy <= 0L))
                {
                    previewCell.Get().userEnergyCellUi.SetShareEnergyText(num, true);
                    userEnergyBar.userEnergyBarUi.ShowAdditionalEnergyLevel(num);
                    shareEnergyDialog.component.TeleportPriceProgressBar.AdditionalProgress = ((float) num) / ((float) eventInstance.TeleportPrice);
                }
                else
                {
                    long num2 = (long) Mathf.Min((float) event3.ExcessEnergy, (float) num);
                    previewCell.Get().userEnergyCellUi.SetShareEnergyText(num2, false);
                    userEnergyBar.userEnergyBarUi.SetSharedEnergyLevel(num2);
                    shareEnergyDialog.component.TeleportPriceProgressBar.AdditionalProgress = ((float) num2) / ((float) eventInstance.TeleportPrice);
                }
            }
        }

        public class EnergyItemNode : Node
        {
            public EnergyItemComponent energyItem;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
            public UserGroupComponent userGroup;
        }

        public class EnergyPreviewCellNode : ShareEnergyScreenSystem.UserEnergyCellNode
        {
            public AdditionalTeleportEnergyPreviewComponent additionalTeleportEnergyPreview;
        }

        public class EnergyPriceEvent : Event
        {
            public long count;
            public long price;
        }

        public class LeagueNode : Node
        {
            public LeagueEnergyConfigComponent leagueEnergyConfig;
            public LeagueConfigComponent leagueConfig;
            public ChestBattleRewardComponent chestBattleReward;
            public LeagueGroupComponent leagueGroup;
        }

        public class SelfExcessEnergyEvent : Event
        {
            public long ExcessEnergy;
        }

        public class SelfSquadLeaderNode : ShareEnergyScreenSystem.SelfUserInSquadNode
        {
            public SquadLeaderComponent squadLeader;
        }

        public class SelfUserInSquadNode : ShareEnergyScreenSystem.UserInSquadNode
        {
            public SelfUserComponent selfUser;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
            public LeagueGroupComponent leagueGroup;
        }

        public class SquadCurrentEnergy : Event
        {
            public long CurrentEnergy;
        }

        public class SquadNode : Node
        {
            public SquadComponent squad;
            public SquadGroupComponent squadGroup;
        }

        public class SquadTeleportPriceEvent : Event
        {
            public long TeleportPrice;
        }

        public class UpdateSelfUserEnergyEvent : Event
        {
        }

        public class UpdateTeleportInfoEvent : Event
        {
        }

        public class UserEnergyBarNode : Node
        {
            public UserEnergyBarUIComponent userEnergyBarUi;
        }

        public class UserEnergyCellNode : Node
        {
            public UserGroupComponent userGroup;
            public UserEnergyCellUIComponent userEnergyCellUi;
        }

        public class UserGiftEnergyEvent : Event
        {
            public long totalGift;
            public List<string> uids = new List<string>();
        }

        public class UserInSquadNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public UserUidComponent userUid;
            public SquadGroupComponent squadGroup;
            public LeagueGroupComponent leagueGroup;
            public BattleEntrancePayerComponent battleEntrancePayer;
        }
    }
}

