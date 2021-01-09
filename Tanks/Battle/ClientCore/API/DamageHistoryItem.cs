namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class DamageHistoryItem
    {
        public Date TimeOfDamage;
        public float Damage;
        public Entity DamagerUser;
    }
}

