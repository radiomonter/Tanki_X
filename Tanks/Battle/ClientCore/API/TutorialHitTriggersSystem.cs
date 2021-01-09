namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class TutorialHitTriggersSystem : ECSSystem
    {
        [OnEventFire]
        public void TankGetHit(HealthChangedEvent e, SelfTankNode selfTank, [JoinAll] SingleNode<TutorialFirstDamageTriggerComponent> firstDamageTrigger)
        {
            if (selfTank.health.CurrentHealth < selfTank.lastHealth.LastHealth)
            {
                firstDamageTrigger.component.GetComponent<TutorialShowTriggerComponent>().Triggered();
            }
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public HealthComponent health;
            public LastHealthComponent lastHealth;
        }
    }
}

