namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CheckMarketItemRestrictionsEvent : Event
    {
        public void MountRestrictByUpgradeLevel(bool value)
        {
            this.MountWillBeRestrictedByUpgradeLevel = this.MountWillBeRestrictedByUpgradeLevel || value;
        }

        public void RestrictByRank(bool value)
        {
            this.RestrictedByRank = this.RestrictedByRank || value;
        }

        public void RestrictByUpgradeLevel(bool value)
        {
            this.RestrictedByUpgradeLevel = this.RestrictedByUpgradeLevel || value;
        }

        public bool RestrictedByRank { get; private set; }

        public bool RestrictedByUpgradeLevel { get; private set; }

        public bool MountWillBeRestrictedByUpgradeLevel { get; private set; }
    }
}

