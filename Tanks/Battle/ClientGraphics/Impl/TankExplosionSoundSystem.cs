namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class TankExplosionSoundSystem : ECSSystem
    {
        [OnEventFire]
        public void PlayDeathSound(NodeAddedEvent evt, TankExplosionNode tank, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            tank.tankExplosionSound.Sound.Play();
        }

        public class TankExplosionNode : Node
        {
            public TankDeadStateComponent tankDeadState;
            public TankExplosionSoundComponent tankExplosionSound;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }
    }
}

