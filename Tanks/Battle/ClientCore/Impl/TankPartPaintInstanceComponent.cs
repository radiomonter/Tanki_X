namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankPartPaintInstanceComponent : Component
    {
        public TankPartPaintInstanceComponent()
        {
        }

        public TankPartPaintInstanceComponent(GameObject paintInstance)
        {
            this.PaintInstance = paintInstance;
        }

        public GameObject PaintInstance { get; set; }
    }
}

