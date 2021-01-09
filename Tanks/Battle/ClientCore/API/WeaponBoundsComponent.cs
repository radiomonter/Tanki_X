namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class WeaponBoundsComponent : Component
    {
        public WeaponBoundsComponent()
        {
        }

        public WeaponBoundsComponent(Bounds weaponBounds)
        {
            this.WeaponBounds = weaponBounds;
        }

        public Bounds WeaponBounds { get; set; }
    }
}

