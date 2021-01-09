namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class MineGraphicsSystem : ECSSystem
    {
        private static readonly float MINE_ACTIVATION_TIME = 1f;
        private static Vector4 MINE_ACTIVATION_COLOR = new Vector4(1f, 1f, 1f, 1f);

        [OnEventFire]
        public void Activation(EffectActivationEvent e, SingleNode<MineEffectComponent> mine)
        {
            mine.Entity.AddComponent(new MineActivationGraphicsComponent(UnityTime.time));
        }

        [OnEventFire]
        public void ActivationEffect(TimeUpdateEvent e, MineActivationNode mine)
        {
            MineConfigComponent mineConfig = mine.mineConfig;
            float num = UnityTime.time - mine.mineActivationGraphics.ActivationStartTime;
            float num2 = num / (MINE_ACTIVATION_TIME * 0.5f);
            if (num2 > 1f)
            {
                num2 = Math.Max((float) 0f, (float) (2f - num2));
            }
            Material material = mine.effectRendererGraphics.Renderer.material;
            material.SetColor("_Color", MINE_ACTIVATION_COLOR);
            material.SetFloat("_ColorLerp", num2);
            if (num > MINE_ACTIVATION_TIME)
            {
                mine.Entity.RemoveComponent<MineActivationGraphicsComponent>();
            }
        }

        [OnEventFire]
        public void AlphaBlendByDistance(TimeUpdateEvent e, MineNode mine, [JoinByTank] EnemyTankNode tank, [JoinByBattle] SelfTankNode selfTank)
        {
            if (!mine.Entity.HasComponent<MineActivationGraphicsComponent>())
            {
                Vector4 vector = MINE_ACTIVATION_COLOR;
                vector.w = MineCommonGraphicsSystem.BlendMine(mine.mineConfig, mine.effectInstance, mine.effectRendererGraphics, selfTank.hullInstance);
                mine.effectRendererGraphics.Renderer.material.SetColor("_Color", vector);
            }
        }

        private void ApplyActivationColor(MineRendererNode mine)
        {
            mine.effectRendererGraphics.Renderer.material.SetColor("_Color", MINE_ACTIVATION_COLOR);
        }

        [OnEventFire]
        public void ApplyActivationColor(NodeAddedEvent e, MineRendererNode mine)
        {
            this.ApplyActivationColor(mine);
        }

        [OnEventFire]
        public void ApplyActivationColor(NodeAddedEvent e, MineRendererPaintedNode mine)
        {
            this.ApplyActivationColor(mine);
        }

        [OnEventFire]
        public void PrepareExplosion(NodeAddedEvent e, MinePrepareExplosionnNode mine)
        {
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public class EnemyTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankComponent tank;
            public EnemyComponent enemy;
        }

        public class MineActivationNode : MineGraphicsSystem.MineNode
        {
            public MineActivationGraphicsComponent mineActivationGraphics;
        }

        public class MineNode : MineGraphicsSystem.MineRendererPaintedNode
        {
            public MineEffectComponent mineEffect;
            public MineConfigComponent mineConfig;
            public EffectInstanceComponent effectInstance;
        }

        public class MinePrepareExplosionnNode : MineGraphicsSystem.MineNode
        {
            public MinePrepareExplosionComponent minePrepareExplosion;
        }

        public class MineRendererNode : Node
        {
            public EffectRendererGraphicsComponent effectRendererGraphics;
        }

        public class MineRendererPaintedNode : MineGraphicsSystem.MineRendererNode
        {
            public EffectPaintedComponent effectPainted;
        }

        public class SelfTankNode : Node
        {
            public TankComponent tank;
            public SelfTankComponent selfTank;
            public HullInstanceComponent hullInstance;
        }
    }
}

