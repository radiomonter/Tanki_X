namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientHUD.API;
    using UnityEngine;

    public class MultikillSystem : ECSSystem
    {
        private Dictionary<int, MultikillUIComponent> multikillNotifications = new Dictionary<int, MultikillUIComponent>();

        private void ActivateEffect(MultikillUIComponent multikillUiComponent, int score, int kills, string userName = "")
        {
            ActivateMultikillNotificationEvent eventInstance = new ActivateMultikillNotificationEvent {
                Score = score,
                Kills = kills,
                UserName = userName
            };
            base.ScheduleEvent(eventInstance, multikillUiComponent.GetComponent<EntityBehaviour>().Entity);
        }

        [OnEventComplete]
        public void ActivateNewNotification(ActivateMultikillNotificationEvent e, SingleNode<MultikillUIComponent> node, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            if (!settings.component.DisableBattleNotifications)
            {
                node.component.ActivateEffect(e.Score, e.Kills, e.UserName);
            }
        }

        [OnEventFire]
        public void DisableOldNotifications(ActivateMultikillNotificationEvent e, SingleNode<MultikillUIComponent> node, [JoinAll, Combine] SingleNode<MultikillUIComponent> multikillUi)
        {
            if (!ReferenceEquals(multikillUi.Entity, node.Entity))
            {
                multikillUi.component.DeactivateEffect();
            }
        }

        [OnEventFire]
        public void DisableOldNotifications(DisableOldMultikillNotificationsEvent e, Node any, [JoinAll, Combine] SingleNode<MultikillUIComponent> multikillUi)
        {
            if (!multikillUi.Entity.Id.Equals(any.Entity.Id))
            {
                multikillUi.component.DeactivateEffect();
            }
        }

        [OnEventFire]
        public void DoubleKill(KillStreakEvent e, SingleNode<TankIncarnationKillStatisticsComponent> node, [JoinByTank] SingleNode<SelfTankComponent> selfTank, [JoinAll] SingleNode<MultikillListComponent> multikillList)
        {
            int kills = node.component.Kills;
            if (this.multikillNotifications.ContainsKey(kills))
            {
                this.ActivateEffect(this.multikillNotifications[kills], e.Score, kills, string.Empty);
            }
            else if (kills > 40)
            {
                this.ActivateEffect(multikillList.component.finalKillStreak, e.Score, kills, string.Empty);
            }
        }

        [OnEventFire]
        public void FillDictionary(NodeAddedEvent e, SingleNode<MultikillListComponent> multikillList)
        {
            this.multikillNotifications.Clear();
            foreach (MultikillElement element in multikillList.component.multikillElements)
            {
                this.multikillNotifications.Add(element.killNumber, element.multikillUiComponent);
            }
        }

        [OnEventFire]
        public void GoldDrop(GoldScheduledNotificationEvent e, Node node, [JoinAll] SingleNode<MultikillListComponent> multikillList)
        {
            this.ActivateEffect(multikillList.component.goldBoxElement, 0, 0, e.Sender);
        }

        [OnEventFire]
        public void InitializeSound(NodeAddedEvent e, MultikillDataNode node)
        {
            node.multikillUi.Voice = (GameObject) node.resourceData.Data;
        }

        [OnEventFire]
        public void LoadMultikillSound(NodeAddedEvent e, SingleNode<MultikillUIComponent> node)
        {
            if ((node.component.VoiceReference != null) && !string.IsNullOrEmpty(node.component.VoiceReference.AssetGuid))
            {
                AssetReferenceComponent component = new AssetReferenceComponent(node.component.VoiceReference);
                node.Entity.AddComponent(component);
                node.Entity.AddComponent<AssetRequestComponent>();
            }
        }

        [OnEventFire]
        public void StreakTermination(StreakTerminationEvent e, SingleNode<SelfBattleUserComponent> battleUser, [JoinAll] StreakTerminationNode streakTermination)
        {
            string str = string.Format(streakTermination.streakTerminationUi.streakTerminationLocalization.Value, e.VictimUid);
            streakTermination.streakTerminationUi.streakTerminationText.text = str;
            base.ScheduleEvent<ActivateMultikillNotificationEvent>(streakTermination);
        }

        public class CombatEventLogNode : Node
        {
            public CombatLogCommonMessagesComponent combatLogCommonMessages;
            public CombatEventLogComponent combatEventLog;
            public UILogComponent uiLog;
            public ActiveCombatLogComponent activeCombatLog;
        }

        public class MultikillDataNode : Node
        {
            public MultikillUIComponent multikillUi;
            public ResourceDataComponent resourceData;
        }

        public class StreakTerminationNode : Node
        {
            public MultikillUIComponent multikillUi;
            public StreakTerminationUIComponent streakTerminationUi;
        }

        public class UserNode : Node
        {
            public UserRankComponent userRank;
            public UserUidComponent userUid;
        }
    }
}

