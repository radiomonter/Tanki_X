namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PriceButtonComponent : MonoBehaviour, Component
    {
        public PriceButtonComponent()
        {
        }

        public PriceButtonComponent(long price)
        {
            this.Price = price;
        }

        public long Price { get; set; }
    }
}

