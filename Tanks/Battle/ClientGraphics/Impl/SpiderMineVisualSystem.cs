namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;

    public class SpiderMineVisualSystem : ECSSystem
    {
        [OnEventFire]
        public void AlfaBlendByDistance(TimeUpdateEvent e, MineBlendNode mine, [JoinByTank] EnemyTankNode isEnemy, [JoinByBattle] SelfTankNode selfTank)
        {
            mine.effectRendererGraphics.Renderer.material.SetFloat(TankMaterialPropertyNames.ALPHA, MineCommonGraphicsSystem.BlendMine(mine.mineConfig, mine.effectInstance, mine.effectRendererGraphics, selfTank.hullInstance));
        }

        public class EnemyTankNode : Node
        {
            public RemoteTankComponent remoteTank;
            public TankGroupComponent tankGroup;
            public EnemyComponent enemy;
        }

        public class MineBlendNode : Node
        {
            public SpiderMineEffectComponent spiderMineEffect;
            public MineConfigComponent mineConfig;
            public EffectInstanceComponent effectInstance;
            public EffectRendererGraphicsComponent effectRendererGraphics;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public HullInstanceComponent hullInstance;
        }
    }
}

