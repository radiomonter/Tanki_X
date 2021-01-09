namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankJumpSoundSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateCommonTankSounds(NodeAddedEvent evt, TankInitNode tank)
        {
            AudioSource sound = Object.Instantiate<AudioSource>(tank.tankJumpSoundPrefab.Sound);
            sound.transform.SetParent(tank.tankVisualRoot.transform, false);
            tank.Entity.AddComponent(new TankJumpSoundComponent(sound));
        }

        [OnEventFire]
        public void PlayJumpSound(NodeAddedEvent evt, TankJumpNode tank, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            tank.tankJumpSound.Sound.Play();
        }

        public class TankInitNode : Node
        {
            public TankVisualRootComponent tankVisualRoot;
            public AssembledTankComponent assembledTank;
            public TankJumpSoundPrefabComponent tankJumpSoundPrefab;
        }

        public class TankJumpNode : Node
        {
            public TankJumpComponent tankJump;
            public TankJumpSoundComponent tankJumpSound;
        }
    }
}

