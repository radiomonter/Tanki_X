namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MagazineLocalStorageComponent : Component
    {
        public MagazineLocalStorageComponent()
        {
        }

        public MagazineLocalStorageComponent(int currentCartridgeCount)
        {
            this.CurrentCartridgeCount = currentCartridgeCount;
        }

        public int CurrentCartridgeCount { get; set; }
    }
}

