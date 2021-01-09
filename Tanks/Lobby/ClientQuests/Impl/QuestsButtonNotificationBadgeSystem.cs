namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientQuests.API;

    public class QuestsButtonNotificationBadgeSystem : ECSSystem
    {
        [OnEventFire]
        public void HideAttentionBadge(NodeRemoveEvent e, QuestsButtonNode questsButtonNode)
        {
            questsButtonNode.notificationBadge.BadgeActivity = false;
        }

        [OnEventFire]
        public void ShowAttentionBadge(NodeAddedEvent e, QuestsButtonNode questsButtonNode, [Combine] CompleteQuestNode quest)
        {
            if (!quest.Entity.HasComponent<RewardedQuestComponent>())
            {
                questsButtonNode.notificationBadge.BadgeActivity = true;
            }
        }

        [OnEventFire]
        public void UpdateButton(NodeRemoveEvent e, SingleNode<RewardedQuestComponent> quest, [JoinAll] QuestsButtonNode button, [JoinAll] ICollection<CompleteQuestNode> quests)
        {
            using (IEnumerator<CompleteQuestNode> enumerator = quests.GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    CompleteQuestNode current = enumerator.Current;
                    if (!ReferenceEquals(current.Entity, quest.Entity) && current.Entity.HasComponent<RewardedQuestComponent>())
                    {
                        return;
                    }
                }
            }
            button.notificationBadge.BadgeActivity = false;
        }

        public class CompleteQuestNode : Node
        {
            public QuestComponent quest;
            public CompleteQuestComponent completeQuest;
        }

        public class QuestsButtonNode : Node
        {
            public QuestsButtonComponent questsButton;
            public NotificationBadgeComponent notificationBadge;
        }
    }
}

