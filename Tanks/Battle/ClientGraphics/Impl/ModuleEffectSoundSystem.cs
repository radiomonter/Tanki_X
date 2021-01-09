namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;

    public class ModuleEffectSoundSystem : ECSSystem
    {
        [OnEventFire]
        public void PlayEffectCreateSound(NodeAddedEvent evt, EffectActivationNode effect, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            effect.effectActivationSound.Sound.Play();
        }

        [OnEventFire]
        public void PlayEffectCreateSound(EffectActivationEvent evt, SingleNode<EffectComponent> effect, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            effect.Entity.AddComponent<EffectReadyForActivationSoundComponent>();
        }

        [OnEventComplete]
        public void PlayEffectRemovingSound(RemoveEffectEvent e, SingleNode<EffectRemovingSoundComponent> effect, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            effect.component.Sound.Play();
        }

        [OnEventFire]
        public void StopEffectCreateSound(NodeRemoveEvent e, SingleNode<EffectActivationSoundComponent> effect)
        {
            effect.component.Sound.Stop();
        }

        public class EffectActivationNode : Node
        {
            public EffectReadyForActivationSoundComponent effectReadyForActivationSound;
            public EffectActivationSoundComponent effectActivationSound;
        }
    }
}

