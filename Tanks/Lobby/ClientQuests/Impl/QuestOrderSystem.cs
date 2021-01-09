namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientQuests.API;

    public class QuestOrderSystem : ECSSystem
    {
        [OnEventFire]
        public void SetQuestOrder(NodeAddedEvent e, NewQuestNode quest)
        {
            QuestOrderComponent component = new QuestOrderComponent {
                Index = quest.slotIndex.Index
            };
            quest.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SetQuestOrder(NodeAddedEvent e, OldQuestNode quest)
        {
            QuestOrderComponent component = new QuestOrderComponent {
                Index = quest.orderItem.Index
            };
            quest.Entity.AddComponent(component);
        }

        public class NewQuestNode : QuestOrderSystem.QuestNode
        {
            public SlotIndexComponent slotIndex;
        }

        public class OldQuestNode : QuestOrderSystem.QuestNode
        {
            public OrderItemComponent orderItem;
        }

        public class QuestNode : Node
        {
            public QuestComponent quest;
            public QuestProgressComponent questProgress;
        }
    }
}

