namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;

    public class MineCommonGraphicsSystem : ECSSystem
    {
        public static float BlendMine(MineConfigComponent config, EffectInstanceComponent effectInstance, EffectRendererGraphicsComponent effectRendererGraphics, HullInstanceComponent selfTankHullInstance)
        {
            float num = 1f;
            float magnitude = (selfTankHullInstance.HullInstance.transform.position - effectInstance.GameObject.transform.position).magnitude;
            if (magnitude > config.BeginHideDistance)
            {
                num = 1f - Math.Min((float) 1f, (float) ((magnitude - config.BeginHideDistance) / config.HideRange));
            }
            effectRendererGraphics.Renderer.enabled = num > 0f;
            return num;
        }

        [OnEventFire]
        public void Explosion(MineExplosionEvent e, MineExplosionNode mine)
        {
            ModuleEffectGraphicsSystem.InstantiateEffectEffect(mine.effectInstance, mine.mineExplosionGraphics.EffectPrefab, mine.mineExplosionGraphics.ExplosionLifeTime, mine.mineExplosionGraphics.Origin);
        }

        public class MineExplosionNode : Node
        {
            public MineExplosionGraphicsComponent mineExplosionGraphics;
            public EffectInstanceComponent effectInstance;
        }
    }
}

