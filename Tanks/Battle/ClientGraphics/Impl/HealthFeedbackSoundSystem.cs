namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;

    public class HealthFeedbackSoundSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableHealthFilter(NodeAddedEvent evt, SelfTankNode tank, SoundListenerNode listener)
        {
            listener.healthFeedbackSoundListener.ResetHealthFeedbackData();
        }

        [OnEventFire]
        public void DisableHealthFilter(NodeRemoveEvent evt, SelfTankNode tank, [JoinAll] SoundListenerNode listener)
        {
            listener.healthFeedbackSoundListener.SwitchToNormalHealthMode();
        }

        [OnEventFire]
        public void DisableHealthFilterOnEnterLobby(LobbyAmbientSoundPlayEvent evt, SoundListenerNode listener)
        {
            listener.healthFeedbackSoundListener.SwitchToNormalHealthMode();
        }

        [OnEventFire]
        public void InitSoundListener(NodeAddedEvent e, SingleNode<HealthFeedbackMapEffectMaterialComponent> mapEffect, SingleNode<HealthFeedbackSoundListenerComponent> listener, SingleNode<GameTankSettingsComponent> settings)
        {
            if (!settings.component.HealthFeedbackEnabled)
            {
                listener.Entity.RemoveComponentIfPresent<HealthFeedbackSoundEffectEnabledComponent>();
            }
            else
            {
                listener.Entity.AddComponentIfAbsent<HealthFeedbackSoundEffectEnabledComponent>();
            }
        }

        [OnEventFire]
        public void SwitchToLowHealthMode(HealthChangedEvent evt, SelfActiveTankNode tank, [JoinAll] SoundListenerNode listener)
        {
            if ((tank.health.CurrentHealth / tank.health.MaxHealth) > listener.healthFeedbackSoundListener.MaxHealthPercentForSound)
            {
                listener.healthFeedbackSoundListener.SwitchToNormalHealthMode();
            }
            else
            {
                listener.healthFeedbackSoundListener.SwitchToLowHealthMode();
            }
        }

        [OnEventFire]
        public void SwitchToNormalMode(NodeAddedEvent evt, SelfDeadTankNode tank, SoundListenerNode listener)
        {
            listener.healthFeedbackSoundListener.SwitchToNormalHealthMode();
        }

        public class HealthFeedbackSoundEffectEnabledComponent : Component
        {
        }

        public class SelfActiveTankNode : HealthFeedbackSoundSystem.SelfTankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class SelfDeadTankNode : HealthFeedbackSoundSystem.SelfTankNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public HealthComponent health;
        }

        public class SoundListenerNode : Node
        {
            public HealthFeedbackSoundListenerComponent healthFeedbackSoundListener;
            public HealthFeedbackSoundSystem.HealthFeedbackSoundEffectEnabledComponent healthFeedbackSoundEffectEnabled;
        }
    }
}

