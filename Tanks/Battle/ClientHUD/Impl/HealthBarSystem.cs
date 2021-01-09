namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientHUD.API;

    public class HealthBarSystem : ECSSystem
    {
        [OnEventFire]
        public void ChangeProgressValueOnAnyHealthBar(HealthChangedEvent e, TankNode tank, [JoinByTank] AttachedHealthBarNode healthBar)
        {
            this.UpdateHealth(tank.health, tank.healthConfig, healthBar.healthBar);
        }

        [OnEventFire]
        public void InitHealthBarProgressOnRemoteTanks(NodeAddedEvent e, RemoteTankNode tank, [Context, JoinByTank] AttachedHealthBarNode healthBar)
        {
            this.UpdateHealth(tank.health, tank.healthConfig, healthBar.healthBar);
        }

        private void UpdateHealth(HealthComponent health, HealthConfigComponent healthConfig, HealthBarComponent healthBar)
        {
            float num = health.CurrentHealth / healthConfig.BaseHealth;
            healthBar.ProgressValue = num;
        }

        public class AttachedHealthBarNode : Node
        {
            public HealthBarComponent healthBar;
            public TankGroupComponent tankGroup;
        }

        public class RemoteTankNode : HealthBarSystem.TankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
            public HealthComponent health;
            public HealthConfigComponent healthConfig;
        }
    }
}

