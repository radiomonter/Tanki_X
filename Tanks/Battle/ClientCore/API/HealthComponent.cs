namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x1b0cf157e6a14a78L)]
    public class HealthComponent : Component
    {
        private float currentHealth;
        private float maxHealth;

        public float CurrentHealth
        {
            get => 
                this.currentHealth;
            set => 
                this.currentHealth = value;
        }

        public float MaxHealth
        {
            get => 
                this.maxHealth;
            set => 
                this.maxHealth = value;
        }
    }
}

