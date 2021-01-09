namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class ArmsRaceRewardNumberComponent : MonoBehaviour, Component
    {
        public AnimatedLong quantity;
        public int initialVal;

        private void Start()
        {
            this.quantity.SetImmediate(-1L);
            this.quantity.Value = this.initialVal;
        }
    }
}

