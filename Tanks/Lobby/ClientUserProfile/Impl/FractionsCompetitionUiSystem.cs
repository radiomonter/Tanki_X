namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.UI;

    public class FractionsCompetitionUiSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Comparison<LeagueEntranceItemComponent> <>f__am$cache0;
        [CompilerGenerated]
        private static Comparison<InvolvedFractionNode> <>f__am$cache1;

        [OnEventFire]
        public void ChangeColorOnFractionAdded(NodeAddedEvent e, SelfUserWithFractionNode user, [JoinByFraction] FractionNode fraction, [Combine, JoinAll] SingleNode<FractionImageColorComponent> image)
        {
            string fractionColor = fraction.fractionInfo.FractionColor;
            this.TryToRecolorImage(image.component.ImagesToRecolor, fractionColor, image.component.DefaultColor);
        }

        [OnEventFire]
        public void ClearColorOnRemoved(NodeRemoveEvent e, SelfUserWithFractionNode user, [JoinByFraction] FractionNode fraction, [Combine, JoinAll] SingleNode<FractionImageColorComponent> image)
        {
            this.RecolorAllImages(image.component.ImagesToRecolor, image.component.DefaultColor);
        }

        [OnEventFire]
        public void FillCurrentCompetitionWindow(NodeAddedEvent e, SingleNode<CurrentCompetitionWindowComponent> window, [JoinAll] Optional<SelfUserWithFractionNode> user)
        {
            window.component.PlayerInfoElement.gameObject.SetActive(user.IsPresent());
        }

        [OnEventFire]
        public void FillFactionContainer(NodeAddedEvent e, SingleNode<FractionContainerComponent> container, [JoinAll] SelfUserNode user, [JoinByFraction] Optional<FractionNode> fraction, [JoinAll] FractionCompetitionNode fractionCompetition)
        {
            Color color;
            FractionContainerComponent component = container.component;
            FractionContainerComponent.FractionContainerTargets target = component.Target;
            if (target == FractionContainerComponent.FractionContainerTargets.PLAYER_FRACTION)
            {
                if (!fraction.IsPresent())
                {
                    component.IsAvailable = false;
                }
                else
                {
                    component.IsAvailable = true;
                    FractionInfoComponent fractionInfo = fraction.Get().fractionInfo;
                    component.FractionTitle = fractionInfo.FractionName;
                    component.FractionLogoUid = fractionInfo.FractionLogoImageUid;
                    color = new Color();
                    component.FractionColor = GetColorFromHex(fractionInfo.FractionColor, color);
                }
            }
            else
            {
                if (target != FractionContainerComponent.FractionContainerTargets.WINNER_FRACTION)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (!fractionCompetition.Entity.HasComponent<FinishedFractionsCompetitionComponent>())
                {
                    component.IsAvailable = false;
                }
                else
                {
                    component.IsAvailable = true;
                    FractionInfoComponent component3 = fractionCompetition.Entity.GetComponent<FinishedFractionsCompetitionComponent>().Winner.GetComponent<FractionInfoComponent>();
                    component.FractionTitle = component3.FractionName;
                    component.FractionLogoUid = component3.FractionLogoImageUid;
                    color = new Color();
                    component.FractionColor = GetColorFromHex(component3.FractionColor, color);
                }
            }
        }

        [OnEventFire]
        public void FillFractionRewardWindow(NodeAddedEvent e, SingleNode<FractionRewardUiComponent> window, [JoinAll] SelfUserWithFractionNode user, [JoinByFraction] FractionNode fraction)
        {
            window.component.RewardImageUid = fraction.fractionInfo.FractionRewardImageUid;
        }

        [OnEventFire]
        public void FillFractionSelectWindow(NodeAddedEvent e, SingleNode<FractionSelectWindowComponent> window, [JoinAll] ICollection<InvolvedFractionNode> fractionsInCompetition)
        {
            Transform root = window.component.FractionDescriptionContainer.transform;
            FractionDescriptionBehaviour fractionDescriptionPrefab = window.component.FractionDescriptionPrefab;
            List<InvolvedFractionNode> list = fractionsInCompetition.ToList<InvolvedFractionNode>();
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = (a, b) => a.fractionInvolvedInCompetition.UserCount.CompareTo(b.fractionInvolvedInCompetition.UserCount);
            }
            list.Sort(<>f__am$cache1);
            root.DestroyChildren();
            foreach (InvolvedFractionNode node in list)
            {
                FractionDescriptionBehaviour behaviour2 = Object.Instantiate<FractionDescriptionBehaviour>(fractionDescriptionPrefab, root);
                behaviour2.FractionTitle = node.fractionInfo.FractionName;
                behaviour2.FractionSlogan = node.fractionInfo.FractionSlogan;
                behaviour2.FractionDescription = node.fractionInfo.FractionDescription;
                behaviour2.LogoUid = node.fractionInfo.FractionLogoImageUid;
                behaviour2.FractionId = node.Entity;
                behaviour2.gameObject.SetActive(true);
            }
        }

        [OnEventFire]
        public void FillLearnMoreWindow(NodeAddedEvent e, SingleNode<FractionLearnMoreWindowComponent> window, [JoinAll] FractionCompetitionNode competition)
        {
            FractionsCompetitionInfoComponent fractionsCompetitionInfo = competition.fractionsCompetitionInfo;
            window.component.CompetitionTitle = fractionsCompetitionInfo.CompetitionTitle;
            window.component.CompetitionDescription = fractionsCompetitionInfo.CompetitionDescription;
            window.component.CompetitionLogoUid = fractionsCompetitionInfo.CompetitionLogoUid;
        }

        [OnEventComplete]
        public void FractionCompetitionStarted(NodeAddedEvent e, FractionCompetitionDialogNode popup, [Combine, JoinByUser] StartNotificationNode notification, [JoinAll] FractionCompetitionNode fractionCompetition, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            FractionsCompetitionInfoComponent fractionsCompetitionInfo = fractionCompetition.fractionsCompetitionInfo;
            PopupDialogComponent popupDialog = popup.popupDialog;
            popupDialog.headerText.text = fractionsCompetitionInfo.CompetitionTitle;
            popupDialog.text.text = fractionsCompetitionInfo.CompetitionStartMessage;
            popupDialog.rewardHeader.text = fractionsCompetitionInfo.MainQuestionMessage;
            popupDialog.buttonText.text = fractionsCompetitionInfo.TakePartButtonText;
            popupDialog.leagueIcon.SpriteUid = fractionsCompetitionInfo.CompetitionLogoUid;
            popupDialog.leagueIcon.GetComponent<Image>().preserveAspect = true;
            popupDialog.itemsContainer.DestroyChildren();
            AnimationTriggerDelayBehaviour component = popupDialog.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>();
            int num = 0;
            foreach (long num2 in fractionCompetition.fractionsCompetitionScores.Scores.Keys)
            {
                FractionInfoComponent component3 = base.GetEntityById(num2).GetComponent<FractionInfoComponent>();
                string fractionName = component3.FractionName;
                string fractionLogoImageUid = component3.FractionLogoImageUid;
                component.dealy = (num++ + 1) * popupDialog.itemsShowDelay;
                LeagueEntranceItemComponent component4 = Object.Instantiate<LeagueEntranceItemComponent>(popupDialog.itemPrefab, popupDialog.itemsContainer, false);
                component4.preview.SpriteUid = fractionLogoImageUid;
                component4.text.text = fractionName;
                component4.gameObject.SetActive(true);
            }
            popup.fractionsCompetitionNotificationDialog.OpenFractionsWindowButton.WillOpen = true;
            popupDialog.Show(!screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators);
        }

        public static Color GetColorFromHex(string colorHex, Color defaultColor = new Color())
        {
            Color color;
            return (!ColorUtility.TryParseHtmlString(colorHex, out color) ? defaultColor : color);
        }

        [OnEventFire]
        public void HideOrShowCompetitionElements(NodeAddedEvent e, SingleNode<FractionsCompetitionHideObjectsComponent> objects, [JoinAll] Optional<FractionCompetitionNode> competition, [JoinAll] Optional<SelfUserWithFractionNode> userWithFraction, [JoinAll] ICollection<InvolvedFractionNode> fractionsInCompetition)
        {
            bool flag2 = userWithFraction.IsPresent();
            bool flag3 = fractionsInCompetition.Count > 0;
            bool flag4 = competition.IsPresent() && (flag2 || flag3);
            foreach (GameObject obj2 in objects.component.ObjectsToHide)
            {
                obj2.SetActive(flag4);
            }
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<FractionsCompetitionButtonComponent> button, [JoinAll, Combine] RewardNotificationNode notification, [JoinAll] FractionCompetitionDialogNode popup)
        {
            popup.popupDialog.Hide();
            base.ScheduleEvent<NotificationShownEvent>(notification);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<FractionsCompetitionButtonComponent> button, [JoinAll, Combine] StartNotificationNode notification, [JoinAll] FractionCompetitionDialogNode popup)
        {
            popup.popupDialog.Hide();
            base.ScheduleEvent<NotificationShownEvent>(notification);
        }

        [OnEventFire]
        public void OnFractionButton(ButtonClickEvent e, SingleNode<FractionButtonComponent> button, [JoinAll] SingleNode<FractionsCompetitionDialogComponent> dialog, [JoinAll] SelfUserNode user)
        {
            Entity fractionEntity = button.component.FractionEntity;
            switch (button.component.Action)
            {
                case FractionButtonComponent.FractionActions.SELECT:
                    if (user.Entity.HasComponent<FractionGroupComponent>())
                    {
                        return;
                    }
                    base.NewEvent<ApplyPlayerToFractionEvent>().Attach(fractionEntity).Attach(user.Entity).Schedule();
                    break;

                case FractionButtonComponent.FractionActions.AWARDS:
                {
                    FractionInfoComponent component = fractionEntity.GetComponent<FractionInfoComponent>();
                    CompetitionAwardWindowComponent competitionAwardWindow = dialog.component.CompetitionAwardWindow;
                    competitionAwardWindow.gameObject.SetActive(true);
                    competitionAwardWindow.FractionName = component.FractionName;
                    competitionAwardWindow.FractionRewardDescription = component.FractionRewardDescription;
                    competitionAwardWindow.RewardImageUid = component.FractionRewardImageUid;
                    Color defaultColor = new Color();
                    competitionAwardWindow.FractionColor = GetColorFromHex(component.FractionColor, defaultColor);
                    break;
                }
                case FractionButtonComponent.FractionActions.LEARN_MORE:
                    dialog.component.LearnMoreWindow.gameObject.SetActive(true);
                    break;

                default:
                    return;
            }
        }

        [OnEventFire]
        public void OnFractionCompetitionButton(ButtonClickEvent e, SingleNode<FractionsCompetitionButtonComponent> button)
        {
            if (button.component.WillOpen)
            {
                base.ScheduleEvent<OpenFractionsCompetitionDialogEvent>(button);
            }
        }

        [OnEventFire]
        public void OnUserWithFractionAdded(NodeAddedEvent e, SelfUserWithFractionNode user, [JoinAll] SingleNode<FractionsCompetitionDialogComponent> dialog, [JoinAll] SingleNode<FractionSelectWindowComponent> window)
        {
            dialog.component.FractionSelectWindow.gameObject.SetActive(false);
            dialog.component.CurrentCompetitionWindow.gameObject.SetActive(true);
        }

        private void RecolorAllImages(Image[] images, Color color)
        {
            foreach (Image image in images)
            {
                image.color = color;
            }
        }

        [OnEventFire]
        public void RefreshFractionColor(NodeAddedEvent e, SingleNode<FractionImageColorComponent> image, [JoinAll] SelfUserNode user, [JoinByFraction] Optional<FractionNode> fraction)
        {
            if (!fraction.IsPresent())
            {
                this.RecolorAllImages(image.component.ImagesToRecolor, image.component.DefaultColor);
            }
            else
            {
                string fractionColor = fraction.Get().fractionInfo.FractionColor;
                this.TryToRecolorImage(image.component.ImagesToRecolor, fractionColor, image.component.DefaultColor);
            }
        }

        [OnEventFire]
        public void RegisterFinished(NodeAddedEvent e, SingleNode<FinishedFractionsCompetitionComponent> node)
        {
        }

        [OnEventFire]
        public void RewardNotification(NodeAddedEvent e, FractionCompetitionDialogNode popup, [JoinByUser] RewardNotificationNode notification, [JoinAll] FractionCompetitionNode fractionCompetition, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            FractionsCompetitionInfoComponent fractionsCompetitionInfo = fractionCompetition.fractionsCompetitionInfo;
            Dictionary<long, int> rewards = notification.fractionsCompetitionRewardNotification.Rewards;
            bool flag = notification.fractionsCompetitionRewardNotification.CrysForWin > 0L;
            PopupDialogComponent popupDialog = popup.popupDialog;
            popupDialog.headerText.text = fractionsCompetitionInfo.CompetitionTitle;
            popupDialog.rewardHeader.text = fractionsCompetitionInfo.HereYourRewardMessage;
            popupDialog.buttonText.text = fractionsCompetitionInfo.RewardsButtonText;
            popupDialog.text.text = !flag ? fractionsCompetitionInfo.LoserFinishMessage : fractionsCompetitionInfo.WinnerFinishMessage;
            string fractionLogoImageUid = base.GetEntityById(notification.fractionsCompetitionRewardNotification.WinnerFractionId).GetComponent<FractionInfoComponent>().FractionLogoImageUid;
            popupDialog.leagueIcon.SpriteUid = fractionLogoImageUid;
            popupDialog.leagueIcon.GetComponent<Image>().preserveAspect = true;
            popupDialog.itemsContainer.DestroyChildren();
            List<LeagueEntranceItemComponent> list = new List<LeagueEntranceItemComponent>();
            foreach (KeyValuePair<long, int> pair in rewards)
            {
                LeagueEntranceItemComponent item = Object.Instantiate<LeagueEntranceItemComponent>(popupDialog.itemPrefab, popupDialog.itemsContainer, false);
                Entity entityById = base.GetEntityById(pair.Key);
                item.preview.SpriteUid = entityById.GetComponent<ImageItemComponent>().SpriteUid;
                bool flag2 = entityById.HasComponent<CrystalItemComponent>();
                long num = !flag2 ? ((long) pair.Value) : (notification.fractionsCompetitionRewardNotification.CrysForWin + ((long) pair.Value));
                bool flag3 = num > 1L;
                item.text.text = entityById.GetComponent<DescriptionItemComponent>().Name + (!flag3 ? string.Empty : " x");
                if (!flag3)
                {
                    item.count.gameObject.SetActive(false);
                }
                else
                {
                    item.count.Value = num;
                    item.count.gameObject.SetActive(true);
                }
                item.gameObject.SetActive(true);
                list.Add(item);
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = (a, b) => a.count.Value.CompareTo(b.count.Value);
            }
            list.Sort(<>f__am$cache0);
            AnimationTriggerDelayBehaviour component = popupDialog.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>();
            for (int i = 0; i < list.Count; i++)
            {
                component.dealy = (i + 1) * popupDialog.itemsShowDelay;
                list[i].transform.SetAsLastSibling();
            }
            popup.fractionsCompetitionNotificationDialog.OpenFractionsWindowButton.WillOpen = false;
            popupDialog.Show(!screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators);
        }

        private void SelectWinner(Entity competitionEntity, FractionScoresContainerComponent container)
        {
            container.WinnerId = !competitionEntity.HasComponent<FinishedFractionsCompetitionComponent>() ? 0L : competitionEntity.GetComponent<FinishedFractionsCompetitionComponent>().Winner.Id;
        }

        [OnEventFire]
        public void ShowDialog(OpenFractionsCompetitionDialogEvent e, [JoinAll] SelfUserNode user, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ICollection<InvolvedFractionNode> fractionsInCompetition)
        {
            FractionsCompetitionDialogComponent component = dialogs.component.Get<FractionsCompetitionDialogComponent>();
            component.Show(null);
            bool flag = !user.Entity.HasComponent<FractionGroupComponent>();
            if ((fractionsInCompetition.Count > 0) && flag)
            {
                component.FractionSelectWindow.gameObject.SetActive(true);
            }
            else
            {
                component.CurrentCompetitionWindow.gameObject.SetActive(true);
            }
        }

        private void TryToRecolorImage(Image[] imagesToRecolor, string fractionHexColor, Color defaultColor)
        {
            Color color;
            bool flag = ColorUtility.TryParseHtmlString(fractionHexColor, out color);
            this.RecolorAllImages(imagesToRecolor, !flag ? defaultColor : color);
        }

        [OnEventFire]
        public void UpdateFractionsScores(NodeAddedEvent e, SingleNode<FractionScoresContainerComponent> container, [JoinAll] FractionCompetitionNode competition)
        {
            container.component.TotalCryFund = competition.fractionsCompetitionScores.TotalCryFund;
            foreach (long num in competition.fractionsCompetitionScores.Scores.Keys)
            {
                Entity entityById = base.GetEntityById(num);
                long scores = competition.fractionsCompetitionScores.Scores[num];
                FractionInfoComponent info = entityById.GetComponent<FractionInfoComponent>();
                container.component.UpdateScores(num, info, scores);
            }
            this.SelectWinner(competition.Entity, container.component);
        }

        [OnEventComplete]
        public void UpdateFractionsScores(UpdateClientFractionScoresEvent e, FractionCompetitionNode competition, [JoinAll] ICollection<SingleNode<FractionScoresContainerComponent>> containers)
        {
            foreach (long num in competition.fractionsCompetitionScores.Scores.Keys)
            {
                Entity entityById = base.GetEntityById(num);
                long scores = competition.fractionsCompetitionScores.Scores[num];
                FractionInfoComponent info = entityById.GetComponent<FractionInfoComponent>();
                foreach (SingleNode<FractionScoresContainerComponent> node in containers)
                {
                    node.component.TotalCryFund = competition.fractionsCompetitionScores.TotalCryFund;
                    node.component.UpdateScores(num, info, scores);
                    this.SelectWinner(competition.Entity, node.component);
                }
            }
        }

        [OnEventFire]
        public void UpdateUserScores(NodeAddedEvent e, SingleNode<FractionUserScoreUiComponent> scores, [JoinAll] SelfUserNode user)
        {
            scores.component.Scores = user.fractionUserScore.TotalEarnedPoints;
        }

        public class FractionCompetitionDialogNode : Node
        {
            public FractionsCompetitionNotificationDialogComponent fractionsCompetitionNotificationDialog;
            public PopupDialogComponent popupDialog;
        }

        public class FractionCompetitionNode : Node
        {
            public FractionsCompetitionInfoComponent fractionsCompetitionInfo;
            public FractionsCompetitionScoresComponent fractionsCompetitionScores;
        }

        public class FractionNode : Node
        {
            public FractionComponent fraction;
            public FractionInfoComponent fractionInfo;
            public FractionGroupComponent fractionGroup;
        }

        public class InvolvedFractionNode : FractionsCompetitionUiSystem.FractionNode
        {
            public FractionInvolvedInCompetitionComponent fractionInvolvedInCompetition;
        }

        public class RewardNotificationNode : Node
        {
            public FractionsCompetitionRewardNotificationComponent fractionsCompetitionRewardNotification;
            public ResourceDataComponent resourceData;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public UserRankComponent userRank;
            public SelfUserComponent selfUser;
            public FractionUserScoreComponent fractionUserScore;
        }

        public class SelfUserWithFractionNode : FractionsCompetitionUiSystem.SelfUserNode
        {
            public FractionGroupComponent fractionGroup;
        }

        public class StartNotificationNode : Node
        {
            public FractionsCompetitionStartNotificationComponent fractionsCompetitionStartNotification;
            public ResourceDataComponent resourceData;
        }
    }
}

