namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics;
    using UnityEngine;

    public class MineCommonSoundsSystem : ECSSystem
    {
        [OnEventComplete]
        public void PlayDeactivationSound(RemoveEffectEvent e, MineNode mine, [JoinByTank] SingleNode<RemoteTankComponent> tank, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            mine.mineSounds.DeactivationSound.Play();
        }

        [OnEventFire]
        public void PlayDropSound(NodeAddedEvent evt, [Combine] MineDropSoundNode mine, SingleNode<MapDustComponent> map)
        {
            MinePlacingTransformComponent minePlacingTransform = mine.minePlacingTransform;
            if (minePlacingTransform.HasPlacingTransform)
            {
                Transform target = minePlacingTransform.PlacingData.transform;
                DustEffectBehaviour effectByTag = map.component.GetEffectByTag(target, minePlacingTransform.PlacingData.textureCoord);
                if (effectByTag != null)
                {
                    AudioSource dropGroundSound;
                    switch (effectByTag.surface)
                    {
                        case DustEffectBehaviour.SurfaceType.Soil:
                        case DustEffectBehaviour.SurfaceType.Sand:
                        case DustEffectBehaviour.SurfaceType.Grass:
                            dropGroundSound = mine.mineSounds.DropGroundSound;
                            break;

                        case DustEffectBehaviour.SurfaceType.Metal:
                        case DustEffectBehaviour.SurfaceType.Concrete:
                            dropGroundSound = mine.mineSounds.DropNonGroundSound;
                            break;

                        default:
                            return;
                    }
                    dropGroundSound.Play();
                }
            }
        }

        [OnEventComplete]
        public void PlayExplosionSound(MineExplosionEvent e, MineNode mine, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            if (mine.mineSounds.ExplosionSound)
            {
                mine.mineSounds.ExplosionSound.Play();
            }
        }

        [OnEventFire]
        public void PrepareMineForDropSound(MineDropEvent evt, SingleNode<MineConfigComponent> mine, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            mine.Entity.AddComponent<MineReadyForDropSoundEffectComponent>();
        }

        public class MineDropSoundNode : MineCommonSoundsSystem.MineNode
        {
            public MinePlacingTransformComponent minePlacingTransform;
            public MineReadyForDropSoundEffectComponent mineReadyForDropSoundEffect;
        }

        public class MineNode : Node
        {
            public MineSoundsComponent mineSounds;
        }
    }
}

