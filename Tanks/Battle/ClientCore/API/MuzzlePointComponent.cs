namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e14935aacaL)]
    public class MuzzlePointComponent : Component
    {
        public int CurrentIndex { get; set; }

        public Transform[] Points { get; set; }

        public Transform Current =>
            this.Points[this.CurrentIndex];
    }
}

