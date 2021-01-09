namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class BonusTakingSoundSystem : ECSSystem
    {
        private const float DESTROY_DELAY = 0.5f;

        [OnEventComplete]
        public void CreateAndPlayBonusTakingSound(NodeAddedEvent e, SingleNode<BrokenBonusBoxInstanceComponent> bonus, [JoinAll] SingleNode<BonusSoundConfigComponent> bonusClientConfig, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            AudioSource bonusTakingSound = bonusClientConfig.component.BonusTakingSound;
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = bonusTakingSound.gameObject,
                AutoRecycleTime = bonusTakingSound.clip.length + 0.5f
            };
            base.ScheduleEvent(eventInstance, bonus);
            Transform instance = eventInstance.Instance;
            Transform transform = bonus.component.Instance.transform;
            instance.position = transform.position;
            instance.rotation = transform.rotation;
            instance.gameObject.SetActive(true);
            instance.GetComponent<AudioSource>().Play();
        }
    }
}

