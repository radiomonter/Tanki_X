namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class WeaponInstanceComponent : Component
    {
        public WeaponInstanceComponent()
        {
        }

        public WeaponInstanceComponent(GameObject weaponInstance)
        {
            this.WeaponInstance = weaponInstance;
        }

        public GameObject WeaponInstance { get; set; }
    }
}

