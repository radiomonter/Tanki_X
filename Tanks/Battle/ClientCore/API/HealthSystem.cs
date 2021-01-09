namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class HealthSystem : ECSSystem
    {
        [OnEventFire]
        public void AddLastHealth(NodeAddedEvent e, SelfTankNode selfTank)
        {
            selfTank.Entity.AddComponent<LastHealthComponent>();
        }

        [OnEventFire]
        public void RemoveLastHealth(NodeRemoveEvent e, SelfTankNode selfTank)
        {
            selfTank.Entity.RemoveComponent<LastHealthComponent>();
        }

        [OnEventComplete]
        public void TankGetHit(HealthChangedEvent e, SelfTankWithLastHealth selfTank)
        {
            selfTank.lastHealth.LastHealth = selfTank.health.CurrentHealth;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public HealthComponent health;
        }

        public class SelfTankWithLastHealth : HealthSystem.SelfTankNode
        {
            public LastHealthComponent lastHealth;
        }
    }
}

