namespace tanks.modules.battle.ClientCore.Scripts.Impl.Tank.Effect.Kamikadze
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.Impl;

    public class KamikadzeEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void EnableEffect(SelfTankExplosionEvent e, SelfTankNode selfTank, [JoinByTank] KamikadzeEffectNode effectNode)
        {
            base.ScheduleEvent<StartSplashEffectEvent>(effectNode);
        }

        public class KamikadzeEffectNode : Node
        {
            public KamikadzeEffectComponent kamikadzeEffect;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankActiveStateComponent tankActiveState;
            public RigidbodyComponent rigidbody;
            public ModuleVisualEffectObjectsComponent moduleVisualEffectObjects;
        }
    }
}

