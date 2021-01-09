namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class GoldNotificationSoundSystem : ECSSystem
    {
        [OnEventFire]
        public void CleanGoldNotification(NodeRemoveEvent e, SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<GoldNotificationPlaySoundComponent> listener)
        {
            listener.Entity.RemoveComponent<GoldNotificationPlaySoundComponent>();
        }

        [OnEventFire]
        public void CreateAndPlayGoldNotificationSound(NodeAddedEvent evt, GoldNotificationSoundListenerNode listener, [JoinAll] SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<GoldSoundConfigComponent> config)
        {
            listener.Entity.RemoveComponent<GoldNotificationPlaySoundComponent>();
            AudioSource goldNotificationSound = config.component.GoldNotificationSound;
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = goldNotificationSound.gameObject,
                AutoRecycleTime = goldNotificationSound.clip.length
            };
            base.ScheduleEvent(eventInstance, listener);
            Transform instance = eventInstance.Instance;
            instance.SetParent(map.component.SceneRoot.transform);
            instance.gameObject.SetActive(true);
            instance.GetComponent<AudioSource>().Play();
        }

        [OnEventFire]
        public void CreateAndPlayGoldNotificationSound(GoldScheduledNotificationEvent e, Node node, [JoinAll] NoGoldNotificationSoundListenerNode listener, [JoinAll] SelfBattleUserNode battleUser, [JoinAll] SingleNode<GoldSoundConfigComponent> config)
        {
            listener.Entity.AddComponent<GoldNotificationPlaySoundComponent>();
        }

        public class GoldNotificationSoundListenerNode : GoldNotificationSoundSystem.SoundListenerNode
        {
            public GoldNotificationPlaySoundComponent goldNotificationPlaySound;
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        [Not(typeof(GoldNotificationPlaySoundComponent))]
        public class NoGoldNotificationSoundListenerNode : GoldNotificationSoundSystem.SoundListenerNode
        {
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
        }
    }
}

