namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;

    public class SpiderMineSoundsSystem : ECSSystem
    {
        [OnEventFire]
        public void StartRunningSound(NodeAddedEvent e, [Combine] ActiveWithTargetSpiderSoundsNode mine, SingleNode<SoundListenerBattleStateComponent> listener)
        {
            this.UpdateRunningSound(mine);
        }

        [OnEventFire]
        public void StopRunningSound(MineExplosionEvent e, SpiderSoundsNode mine)
        {
            mine.spiderMineSounds.RunSoundController.FadeOut();
        }

        [OnEventFire]
        public void StopRunningSound(RemoveEffectEvent e, SpiderSoundsNode mine)
        {
            mine.spiderMineSounds.RunSoundController.FadeOut();
        }

        [OnEventFire]
        public void StopRunningSound(NodeRemoveEvent e, SingleNode<UnitTargetComponent> node, [JoinSelf] ActiveSpiderSoundsNode spider)
        {
            spider.spiderMineSounds.RunSoundController.FadeOut();
        }

        private void UpdateRunningSound(ActiveWithTargetSpiderSoundsNode mine)
        {
            if (mine.unitTarget.Target.HasComponent<RigidbodyComponent>())
            {
                if (!mine.spiderAnimator.OnGround)
                {
                    mine.spiderMineSounds.RunSoundController.FadeOut();
                }
                else
                {
                    mine.spiderMineSounds.RunSoundController.FadeIn();
                }
            }
        }

        [OnEventFire]
        public void UpdateRunningSound(UpdateEvent e, ActiveWithTargetSpiderSoundsNode mine, [JoinAll] SingleNode<SoundListenerBattleStateComponent> listener)
        {
            this.UpdateRunningSound(mine);
        }

        public class ActiveSpiderSoundsNode : SpiderMineSoundsSystem.SpiderSoundsNode
        {
            public EffectActiveComponent effectActive;
        }

        public class ActiveWithTargetSpiderSoundsNode : SpiderMineSoundsSystem.ActiveSpiderSoundsNode
        {
            public UnitTargetComponent unitTarget;
        }

        public class SpiderSoundsNode : Node
        {
            public SpiderMineSoundsComponent spiderMineSounds;
            public SpiderAnimatorComponent spiderAnimator;
        }
    }
}

