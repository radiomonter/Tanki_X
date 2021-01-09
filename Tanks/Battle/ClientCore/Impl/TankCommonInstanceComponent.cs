namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankCommonInstanceComponent : Component
    {
        public TankCommonInstanceComponent()
        {
        }

        public TankCommonInstanceComponent(GameObject tankCommonInstance)
        {
            this.TankCommonInstance = tankCommonInstance;
        }

        public GameObject TankCommonInstance { get; set; }
    }
}

